using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Handlers.Interfaces;
using MyLibrary.Server.Http.Responses;
using System.Runtime.CompilerServices;

namespace MyLibrary.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseHandler<Warehouse> _warehouseHandler;
        private readonly IResultHandler<ITaskResult> _resultHandler;

        public WarehouseController(IWarehouseHandler<Warehouse> warehouseHandler, IResultHandler<ITaskResult> resultHandler)
        {
            _warehouseHandler = warehouseHandler;
            _resultHandler = resultHandler;
        }

        [HttpGet("getStock")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStock([FromQuery] string isbn)
        {
            if(string.IsNullOrWhiteSpace(isbn))
            {
                return BadRequest(new WarehouseTaskResult(succeeded: false, message: "ISBN is required.", statusCode: StatusCodes.Status400BadRequest));
            }
            var result = await _warehouseHandler.GetStockAsync(isbn);
            return _resultHandler.ReadResult(result);
        }

        [HttpGet("getAllStocks")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllStocks()
        {
            var result = await _warehouseHandler.GetAllStocksAsync();
            return _resultHandler.ReadResult(result);
        }

        [HttpPost("createStock")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateStock([FromBody] WarehouseDTO warehouseDto)
        {
            if (warehouseDto == null)
            {
                return BadRequest(new WarehouseTaskResult(succeeded: false, message: "Warehouse cannot be null.", statusCode: StatusCodes.Status400BadRequest));
            }
            var result = await _warehouseHandler.CreateStockAsync(warehouseDto);
            return _resultHandler.ReadResult(result);
        }

        [HttpPost("addStock")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status304NotModified)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddStock([FromBody] WarehouseDTO warehouseDto)
        {
            if (warehouseDto == null)
            {
                return BadRequest(new WarehouseTaskResult(succeeded: false, message: "Warehouse cannot be null.", statusCode: StatusCodes.Status400BadRequest));
            }
            var result = await _warehouseHandler.AddStockAsync(warehouseDto);
            return _resultHandler.ReadResult(result);
        }

        [HttpPut("updateStock")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateStock([FromBody] WarehouseDTO warehouseDto)
        {
            if (warehouseDto == null)
            {
                return BadRequest(new WarehouseTaskResult(succeeded: false, message: "Warehouse cannot be null.", statusCode: StatusCodes.Status400BadRequest));
            }
            var result = await _warehouseHandler.UpdateStockAsync(warehouseDto);
            return _resultHandler.ReadResult(result);
        }

        [HttpDelete("deleteStock")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteStock([FromQuery] int id)
        {
            if (id <= 0)
            {
                return BadRequest(new WarehouseTaskResult(succeeded: false, message: "ISBN cannot be null.", statusCode: StatusCodes.Status400BadRequest));
            }
            var result = await _warehouseHandler.DeleteStockAsync(id);
            return _resultHandler.ReadResult(result);
        }

        [HttpPut("removeStock")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status304NotModified)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveStock(WarehouseDTO warehouseDto)
        {
            if (warehouseDto == null)
            {
                return BadRequest(new WarehouseTaskResult(succeeded: false, message: "Warehouse cannot be null.", statusCode: StatusCodes.Status400BadRequest));
            }
            var result = await _warehouseHandler.RemoveStockAsync(warehouseDto);
            return _resultHandler.ReadResult(result);
        }
    }
}
