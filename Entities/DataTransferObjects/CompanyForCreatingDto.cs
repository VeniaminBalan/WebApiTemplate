﻿using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects;

public class CompanyForCreatingDto // Aka CompanyRequest
{
    public string Name { get; set; }
    public string Address { get; set; } 
    public string Country { get; set; } 
    
    public IEnumerable<EmployeeForCreationDto>? Employees { get; set; }
}