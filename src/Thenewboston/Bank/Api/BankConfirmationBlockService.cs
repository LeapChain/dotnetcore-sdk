﻿using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Thenewboston.Bank.Api.Models;
using Thenewboston.Common.Api.Models;
using Thenewboston.Common.Http;
using Thenewboston.Common.Models;

namespace Thenewboston.Bank.Api
{
    public class BankConfirmationBlockService : IBankConfirmationBlockService
    {
        private readonly IHttpRequestSender _requestSender;

        public BankConfirmationBlockService(IHttpRequestSender requestSender)
        {
            _requestSender = requestSender;
        }

        /// <summary>
        /// Retrieves confirmation blocks from the connected bank via paginated <see cref="PaginatedResponseModel"/>
        /// </summary>
        /// <returns><see cref="PaginatedResponseModel"/> containing all bank to client confirmation blocks</returns>
        public async Task<PaginatedResponseModel<BankConfirmationBlock>> GetAllBankConfiramtionBlocksAsync(
            int offset = 0, int limit = 10)
        {
            var response = await _requestSender.GetAsync($"/confirmation_blocks?offset={offset}&limit={limit}"); 
            
            if(!response.IsSuccessStatusCode)
            {
                // TODO: Create specific exception
                throw new Exception(); 
            }

            var stringResponse = await response.Content.ReadAsStringAsync(); 

            if(string.IsNullOrEmpty(stringResponse))
            {
                // TODO: Create specific exception
                throw new Exception(); 
            }

            var result = JsonConvert.DeserializeObject<PaginatedResponseModel<BankConfirmationBlock>>(stringResponse);

            return result; 
        }

        /// NOTE: Not implemented in the app layer as this is functionality for the validator, therefore not needed in any apps.
        /// <summary>
        /// Provides a facility for a confirmation validator to post a new <see cref="ConfirmationBlock"/> to the 
        /// receiving bank. 
        /// </summary>
        /// <param name="confirmationBlock"></param>
        /// <returns><see cref="HttpResponseMessage"/></returns>
        public async Task<HttpResponseMessage> PostConfirmationBlockAsync(ConfirmationBlock confirmationBlock)
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(confirmationBlock), Encoding.UTF8, "application/json"); 
            var request = await _requestSender.PostAsync("/confirmation_blocks", httpContent); 

            if(!request.IsSuccessStatusCode)
            {
                // TODO: Create specific exception
                throw new Exception(); 
            }

            var response = new HttpResponseMessage(System.Net.HttpStatusCode.Created);
            return response; 
        }
    }
}
