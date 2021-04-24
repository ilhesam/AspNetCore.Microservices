using System.Collections.Generic;
using System.Linq;
using Hotel.ApiGateways.Main.Areas.Financial.Common;
using Hotel.ApiGateways.Main.Areas.Financial.ViewModels.FinancialTransaction.Create;
using Hotel.ApiGateways.Main.Areas.Financial.ViewModels.FinancialTransaction.Get;
using Hotel.ApiGateways.Main.Areas.Financial.ViewModels.FinancialTransaction.List;
using Hotel.ApiGateways.Main.Common.ViewModels;
using Hotel.Common.Extensions;
using Hotel.Common.Features.Financial.FinancialTransactionFeatures.Commands.Create;
using Hotel.Common.Features.Financial.FinancialTransactionFeatures.Queries.List;
using Hotel.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Hotel.ApiGateways.Main.Areas.Financial.Controllers
{
    public class FinancialTransactionsController : FinancialAreaApiController
    {
        [HttpGet]
        public IActionResult List()
        {
            FinancialTransactionsResponse response;
            FinancialTransactionsResponseData responseData;
            List<FinancialTransactionDto> list;

            var query = new ListFinancialTransactionsQuery();
            var result = RabbitMq.Publish<ListFinancialTransactionsQuery, Result<ListFinancialTransactionsResult>>(query);

            if (!result.IsSucceeded)
            {
                return BadRequest(new ApiResponse(result.Error.Code, result.Error.Description));
            }

            list = result.Data.FinancialTransactions.ConvertJsonObjectListTo<FinancialTransactionDto>();
            responseData = new FinancialTransactionsResponseData(list);
            response = new FinancialTransactionsResponse("OK", "Success", responseData);
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateFinancialTransactionInput input)
        {
            ApiResponse response;

            var command = new CreateFinancialTransactionCommand(input.About, input.Amount, (byte)input.Type);
            var result = RabbitMq.Publish<CreateFinancialTransactionCommand, Result<CreateFinancialTransactionResult>>(command);

            if (!result.IsSucceeded)
            {
                response = new ApiResponse(result.Error.Code, result.Error.Description);
                return BadRequest(response);
            }

            response = new ApiResponse("OK", "Success");
            return Ok(response);
        }
    }
}