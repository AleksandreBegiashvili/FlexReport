﻿using FlexReport.Application.Common.Interfaces.External;
using FlexReport.Application.Common.Models;
using Microsoft.Data.SqlClient;

namespace FlexReport.Infrastructure.Integrations.DataAccess;

public class SqlServerDataAccess : IDataAccess
{
    public async Task<GetDataResponse> GetData(GetDataRequest request)
    {
        await using var connection = new SqlConnection(request.ConnectionString);
        connection.Open();

        var query = request.Query.Replace(";", string.Empty);
        var countQuery = BuildCountQuery(query);
        var pagedQuery = BuildPagedQuery(query, request.Page, request.PageSize);

        await using var command = new SqlCommand(countQuery, connection);

        var totalCount = await command.ExecuteScalarAsync() as int? ?? 0;

        command.CommandText = pagedQuery;

        await using var dataReader = await command.ExecuteReaderAsync();

        var data = new List<SqlServerDataRow>();
        var headers = new List<string?>();
        var isFirstIteration = true;

        while (await dataReader.ReadAsync())
        {
            var columnCount = dataReader.VisibleFieldCount;
            var rowValues = new List<string?>();

            if (isFirstIteration)
            {
                headers = GetColumnHeaders(dataReader);
            }

            for (var i = 0; i < columnCount; i++)
            {
                var fieldValue = dataReader.GetFieldValue<object>(i);
                var stringRepresentation = fieldValue.ToString();
                rowValues.Add(stringRepresentation);
            }

            data.Add(new SqlServerDataRow(rowValues));
            isFirstIteration = false;
        }

        return new GetDataResponse(headers, data, totalCount);
    }

    private static List<string?> GetColumnHeaders(SqlDataReader dataReader)
    {
        var headerValues = new List<string?>();

        for (var i = 0; i < dataReader.VisibleFieldCount; i++)
        {
            var fieldName = dataReader.GetName(i);
            headerValues.Add(fieldName);
        }

        return headerValues;
    }

    private static string BuildPagedQuery(string query, int page, int pageSize)
    {
        var offset = (page - 1) * pageSize;
        var limit = pageSize;
        return @$"SELECT * FROM
                  ({query}) m
                  ORDER BY (SELECT NULL)
                  OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY";
    }

    private static string BuildCountQuery(string query)
    {
        return @$"SELECT COUNT(*) FROM ({query}) m";
    }
}