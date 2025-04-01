using Microsoft.AspNetCore.Mvc;

namespace calculator_api_rest.Controllers;

[ApiController]
[Route("[controller]")]
public class CalculatorController : ControllerBase
{
    private readonly ILogger<CalculatorController> _logger;

    public CalculatorController(ILogger<CalculatorController> logger)
    {
        _logger = logger;
    }

    private IActionResult PerformOperation(string firstNumber, string secondNumber, Func<decimal, decimal, decimal> operation)
    {
        if (!decimal.TryParse(firstNumber, out var firstDecimal) || !decimal.TryParse(secondNumber, out var secondDecimal))
            return BadRequest("Invalid Input");

        var result = operation(firstDecimal, secondDecimal);
        return Ok(result.ToString());
    }

    [HttpGet("sum/{firstNumber}/{secondNumber}")]
    public IActionResult GetSum(string firstNumber, string secondNumber)
    {
        return PerformOperation(firstNumber, secondNumber, (a, b) => a + b);
    }

    [HttpGet("subtract/{firstNumber}/{secondNumber}")]
    public IActionResult GetSubtract(string firstNumber, string secondNumber)
    {
        return PerformOperation(firstNumber, secondNumber, (a, b) => a - b);
    }

    [HttpGet("div/{firstNumber}/{secondNumber}")]
    public IActionResult GetDiv(string firstNumber, string secondNumber)
    {
        try
        {
            return PerformOperation(firstNumber, secondNumber, (a, b) => a / b);
        }
        catch (DivideByZeroException e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest("Division by zero is not allowed.");
        }
    }

    [HttpGet("multiply/{firstNumber}/{secondNumber}")]
    public IActionResult GetMultiply(string firstNumber, string secondNumber)
    {
        return PerformOperation(firstNumber, secondNumber, (a, b) => a * b);
    }
    
    [HttpGet("average/{firstNumber}/{secondNumber}")]
    public IActionResult GetAverage(string firstNumber, string secondNumber)
    {
        return PerformOperation(firstNumber, secondNumber, (a, b) => (a + b) / 2);
    }

    [HttpGet("square-root/{number}")]
    public IActionResult GetSquareRoot(string number)
    {
        if (!double.TryParse(number, out var numberDecimal))
            return BadRequest("Invalid Input");
        if (numberDecimal < 0)
            return BadRequest("Invalid Input");
        var result = Math.Sqrt(numberDecimal);
        return Ok(result.ToString());
    }

    // crie o método GetSquare que retorna o quadrado de um número
    [HttpGet("square/{number}")]
    public IActionResult GetSquare(string number)
    {
        return PerformOperation(number, "2", (a, b) => (decimal)Math.Pow((double)a, (double)b));
    }
}
