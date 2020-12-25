﻿using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Thenewboston.Common.Api.Models;
using Thenewboston.Common.Http;
using Thenewboston.Validator.Api.Models;
using Thenewboston.Validator.Models;

namespace Thenewboston.Validator.Api
{
    public class ValidatorsService : IValidatorsService
    {
        private readonly IHttpRequestSender _requestSender;

        public ValidatorsService(IHttpRequestSender requestSender)
        {
            _requestSender = requestSender;
        }

        public async Task<PaginatedResponseModel<ValidatorResponseModel>> GetAllValidatorsAsync(
            int offset = 0,
            int limit = 10)
        {
            var response = await _requestSender.GetAsync($"/validators?offset={offset}&limit={limit}");

            if (!response.IsSuccessStatusCode)
            {
                //TODO: create specific exception
                throw new Exception();
            }

            var stringResult = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(stringResult))
            {
                //TODO: create specific exception
                throw new Exception();
            }

            var result = JsonConvert.DeserializeObject<PaginatedResponseModel<ValidatorResponseModel>>(stringResult);

            return result;
        }
    }
}
