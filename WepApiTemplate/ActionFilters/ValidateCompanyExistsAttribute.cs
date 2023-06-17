using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WepApiTemplate.ActionFilters;

public class ValidateCompanyExistsAttribute : IAsyncActionFilter
{
    private readonly IRepositoryManager _repository; 
    private readonly ILoggerManager _logger; 
    public ValidateCompanyExistsAttribute(IRepositoryManager repository, ILoggerManager logger) 
    { 
        _repository = repository; 
        _logger = logger; 
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var trackChanhes = context.HttpContext.Request.Method.Equals("PUT");
        var id = (Guid)context.ActionArguments["id"];
        var company = await _repository.Company.GetByIdAsync(id, trackChanhes);

        if (company is null)
        {
            _logger.LogInfo($"Company with id: {id} doesn't exist in the database."); 
            context.Result = new NotFoundResult();
        }
        
        context.HttpContext.Items.Add("company", company); 
        await next(); 
    }
}