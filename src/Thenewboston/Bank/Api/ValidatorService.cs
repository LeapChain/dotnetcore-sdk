﻿using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Thenewboston.Bank.Api.Models;
using Thenewboston.Bank.Models;
using Thenewboston.Common.Api.Models;
using Thenewboston.Common.Http;

namespace Thenewboston.Bank.Api
{
    public class ValidatorService : IValidatorService
    {
        private readonly IHttpRequestSender _requestSender;

        public ValidatorService(IHttpRequestSender requestSender)
        {
            _requestSender = requestSender;
        }

        public async Task<PaginatedResponseModel<BankValidator>> GetAllValidatorsAsync(int offset = 0, int limit = 10)
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

            var result = JsonConvert.DeserializeObject<PaginatedResponseModel<BankValidator>>(stringResult);

            return result;
        }


        //TODO: remove this
        public async Task<BankValidator> PatchValidatorAsync(string nodeIdentifier, RequestModel trust)
        {
            var jsonTrust = JsonConvert.SerializeObject(trust);
            var httpContent = new StringContent(jsonTrust, Encoding.UTF8, "application/json");

            var response = await _requestSender.PatchAsync($"/validators/{nodeIdentifier}", httpContent);

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

            var result = JsonConvert.DeserializeObject<BankValidator>(stringResult);

            return result;
        }
    }
}
