using Hotel.RabbitMQ;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Text;
using Transaction.Dto;
using Transaction.Repository;

namespace Transaction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRespository;
        private readonly IRabbitMQConsumer _rabbitmqConsumer;

        public TransactionController(ITransactionRepository transactionRepository, IRabbitMQConsumer rabbitmqConsumer)
        {
            _transactionRespository = transactionRepository;
            _rabbitmqConsumer = rabbitmqConsumer;
        }
        [HttpGet("bookings")]
        public async Task<IActionResult> Get()
        {
            ResponseDto response;
            try
            {
                string message;

                var lstTransactions = await _transactionRespository.GetAllTransactions();

                if (lstTransactions.Any())
                {
                    message = "Records found.";
                }
                else
                {
                    message = "No records found.";
                }
                response = new ResponseDto()
                {
                    Success = true,
                    Message = message,
                    Payload = lstTransactions
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new ResponseDto()
                {
                    Success = false,
                    Message = ex.Message,
                    Payload = null
                };
                return Ok(response);
            }
        }

        [HttpGet("{user}")]
        public async Task<IActionResult> GetTransactionsByUserId(int userId)
        {
            ResponseDto response;
            try
            {
                string message;

                var lstTransactions = await _transactionRespository.GetUserTransactions(userId);

                if (lstTransactions.Any())
                {
                    message = "Records found.";
                }
                else
                {
                    message = "No records found.";
                }
                response = new ResponseDto()
                {
                    Success = true,
                    Message = message,
                    Payload = lstTransactions
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new ResponseDto()
                {
                    Success = false,
                    Message = ex.Message,
                    Payload = null
                };
                return Ok(response);
            }
        }

    }
}
