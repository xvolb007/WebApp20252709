using Application.Interaces;
using Application.Services;
using Domain.DTOs;
using Domain.QueueTasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApp20252709.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculationController : ControllerBase
    {
        private readonly CalculationService _calculationService;
        private readonly IRabbitMqProducer _rabbitMqProducer;


        public CalculationController(CalculationService calculationService, IRabbitMqProducer rabbitMqProducer)
        {
            _calculationService = calculationService;
            _rabbitMqProducer = rabbitMqProducer;
        }
        [HttpPost("{key:int}")]
        public async Task<IActionResult> Calculate(int key, [FromBody] CalculationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = _calculationService.Calculate(key, request.Input);

            var task = new CalculationTask
            {
                Key = key,
                ComputedValue = response.ComputedValue,
                InputValue = response.InputValue,
                PreviousValue = response.PreviousValue
            };

            await _rabbitMqProducer.Publish(CalculationTask.QueueName, task);

            return Ok(response);
        }
    }
}
