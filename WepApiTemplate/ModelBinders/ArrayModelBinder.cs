using System.ComponentModel;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WepApiTemplate.ModelBinders;

public class ArrayModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        //check if  parameter is the same type.
        if(!bindingContext.ModelMetadata.IsEnumerableType) 
        { 
            bindingContext.Result = ModelBindingResult.Failed(); 
            return Task.CompletedTask; 
        } 
 
        //extract the value (a comma-separated string of GUIDs)
        var providedValue = bindingContext.ValueProvider 
            .GetValue(bindingContext.ModelName) 
            .ToString(); 
        if(string.IsNullOrEmpty(providedValue)) 
        { 
            bindingContext.Result = ModelBindingResult.Success(null); 
            return Task.CompletedTask; 
        } 
        
        //store the type the IEnumerable consists of (GUID).
        var genericType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0]; 
        
        //create a converter to a GUID type
        var converter = TypeDescriptor.GetConverter(genericType); 
 
        //object (objectArray) that consist of all the GUID values we sent to the API
        var objectArray = providedValue.Split(new[] { "," },  
                StringSplitOptions.RemoveEmptyEntries) 
            .Select(x => converter.ConvertFromString(x.Trim())) 
            .ToArray(); 
 
        var guidArray = Array.CreateInstance(genericType, objectArray.Length); 
        objectArray.CopyTo(guidArray, 0); 
        bindingContext.Model = guidArray; 
 
        bindingContext.Result = ModelBindingResult.Success(bindingContext.Model); 
        return Task.CompletedTask; 
    }
}