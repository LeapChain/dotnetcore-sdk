﻿using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Moq;
using Newtonsoft.Json;
using Thenewboston.Bank.Api;
using Thenewboston.Bank.Api.Models;
using Thenewboston.Bank.Models;
using Thenewboston.Common.Api.Models;
using Thenewboston.Common.Http;
using Xunit;

namespace Thenewboston.Tests.Bank.Api
{
    public class ConnectedBanksServiceTests
    {
        public class GetBanksAsync
        {
            [Fact]
            public async void ListOfBanksIsReturned()
            {
                var expectedResponseModel = new PaginatedResponseModel<BankNodeResponse>
                {
                    Count = 2,
                    Next = null,
                    Previous = null,
                    Results =
                    new List<BankNodeResponse>
                    {
                        new BankNodeResponse
                        {
                            AccountNumber = "5e12967707909e62b2bb2036c209085a784fabbc3deccefee70052b6181c8ed8",
                            IpAddress = "83.168.1.232",
                            NodeIdentifier = "d5356888dc9303e44ce52b1e06c3165a7759b9df1e6a6dfbd33ee1c3df1ab4d1",
                            Port = 80,
                            Protocol = "http",
                            Version = "v1.0",
                            DefaultTransactionFee = 1,
                            Trust = "100.00"
                        },
                        new BankNodeResponse
                        {
                            AccountNumber = "db1a9ac3c356ab744ab4ad5256bb86c2f6dfaa7c1aece1f026a08dbd8c7178f2",
                            IpAddress = "74.124.1.68",
                            NodeIdentifier = "3214108063cda7b259782c57ff8cec343ad2f1ad35baf38c3503db5cf6f3b2f7",
                            Port = 80,
                            Protocol = "http",
                            Version = "v1.0",
                            DefaultTransactionFee = 2,
                            Trust = "98.32"
                        }
                    }
                };

                var service = BuildGetBanksAsyncConnectedBanksServiceMock(expectedResponseModel);

                var banks = await service.GetBanksAsync(0, 10);

                var expectedResponseModelStr = JsonConvert.SerializeObject(expectedResponseModel);
                var actualResponseModelStr = JsonConvert.SerializeObject(banks);
                Assert.Equal(expectedResponseModelStr, actualResponseModelStr);
            }
        }

        public class UpdateBankAsync
        {
            [Fact]
            public async void UpdatedBankIsReturned()
            {
                var expectedBank = new BankNodeResponse
                {
                    AccountNumber = "5e12967707909e62b2bb2036c209085a784fabbc3deccefee70052b6181c8ed8",
                    IpAddress = "83.168.1.232",
                    NodeIdentifier = "d5356888dc9303e44ce52b1e06c3165a7759b9df1e6a6dfbd33ee1c3df1ab4d1",
                    Port = 80,
                    Protocol = "http",
                    Version = "v1.0",
                    DefaultTransactionFee = 1,
                    Trust = "53.22"
                };

                var service = BuildUpdateBankAsyncConnectedBanksServiceMock(expectedBank);

                var bank = await service.UpdateBankAsync(
                    "d5356888dc9303e44ce52b1e06c3165a7759b9df1e6a6dfbd33ee1c3df1ab4d1",
                    new RequestModel());

                var expectedBankStr = JsonConvert.SerializeObject(expectedBank);
                var actualBankStr = JsonConvert.SerializeObject(bank);
                Assert.Equal(expectedBankStr, actualBankStr);
            }
        }

        public static IConnectedBanksService BuildGetBanksAsyncConnectedBanksServiceMock(PaginatedResponseModel<BankNodeResponse> expectedResponseModel)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(expectedResponseModel), Encoding.UTF8, "application/json");

            var requestSenderMock = new Mock<IHttpRequestSender>();
            IConnectedBanksService service = new ConnectedBanksService(requestSenderMock.Object);

            requestSenderMock
                .Setup(x => x.GetAsync("/banks"))
                .ReturnsAsync(response);

            return service;
        }

        public static IConnectedBanksService BuildUpdateBankAsyncConnectedBanksServiceMock(BankNodeResponse expectedBank)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(expectedBank), Encoding.UTF8, "application/json");

            var requestSenderMock = new Mock<IHttpRequestSender>();
            IConnectedBanksService service = new ConnectedBanksService(requestSenderMock.Object);

            requestSenderMock
                .Setup(x => x.PatchAsync(It.IsAny<string>(), It.IsAny<StringContent>()))
                .ReturnsAsync(response);

            return service;
        }
    }
}
