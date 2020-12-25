﻿using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Moq;
using Newtonsoft.Json;
using Thenewboston.Bank.Models;
using Thenewboston.Common.Api.Models;
using Thenewboston.Common.Http;
using Thenewboston.Validator.Api;
using Thenewboston.Validator.Models;
using Xunit;

namespace Thenewboston.Tests.Validator.Api
{
    public class ConnectedBanksServiceTests
    {
        public class GetBanksAsync
        {
            [Fact]
            public async void ListOfBanksIsReturned()
            {
                var expectedResponseModel = new PaginatedResponseModel<ValidatorBank>
                {
                    Count = 2,
                    Next = null,
                    Previous = null,
                    Results =
                    new List<ValidatorBank>
                    {
                        new ValidatorBank
                        {
                            AccountNumber = "5e12967707909e62b2bb2036c209085a784fabbc3deccefee70052b6181c8ed8",
                            IpAddress = "83.168.1.232",
                            NodeIdentifier = "d5356888dc9303e44ce52b1e06c3165a7759b9df1e6a6dfbd33ee1c3df1ab4d1",
                            Port = 80,
                            Protocol = "http",
                            Version = "v1.0",
                            DefaultTransactionFee = 1,
                            ConfirmationExpiration = null,
                            Trust = "100.00"
                        },
                        new ValidatorBank
                        {
                            AccountNumber = "db1a9ac3c356ab744ab4ad5256bb86c2f6dfaa7c1aece1f026a08dbd8c7178f2",
                            IpAddress = "74.124.1.68",
                            NodeIdentifier = "3214108063cda7b259782c57ff8cec343ad2f1ad35baf38c3503db5cf6f3b2f7",
                            Port = 80,
                            Protocol = "http",
                            Version = "v1.0",
                            DefaultTransactionFee = 2,
                            ConfirmationExpiration = null,
                            Trust = "98.32"
                        }
                    }
                };

                var service = BuildGetBanksAsyncValidatorServiceMock(expectedResponseModel);

                var banks = await service.GetBanksAsync(0, 10);

                var expectedResponseModelStr = JsonConvert.SerializeObject(expectedResponseModel);
                var actualResponseModelStr = JsonConvert.SerializeObject(banks);
                Assert.Equal(expectedResponseModelStr, actualResponseModelStr);
            }
        }

        public static IConnectedBanksService BuildGetBanksAsyncValidatorServiceMock(PaginatedResponseModel<ValidatorBank> expectedResponseModel)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(expectedResponseModel), Encoding.UTF8, "application/json");

            var requestSenderMock = new Mock<IHttpRequestSender>();
            IConnectedBanksService service = new ConnectedBanksService(requestSenderMock.Object);

            requestSenderMock
                .Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(response);

            return service;
        }
    }
}
