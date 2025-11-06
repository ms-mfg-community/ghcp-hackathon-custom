#!/usr/bin/env dotnet-script
#r "nuget: Microsoft.EntityFrameworkCore.Sqlite, 8.0.0"

using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

// Add reference to the test project
#r "calculator.tests/bin/Debug/net9.0/calculator.tests.dll"

using calculator.tests.Data;

Console.WriteLine("=".PadRight(80, '='));
Console.WriteLine("ALL TEST CASES IN DATABASE");
Console.WriteLine("=".PadRight(80, '='));
Console.WriteLine();

using (var helper = new TestDataHelper())
{
    var testCases = helper.GetAllTestCases().ToList();

    Console.WriteLine($"Total Test Cases: {testCases.Count}\n");

    var categories = testCases.GroupBy(tc => tc.Category);

    foreach (var categoryGroup in categories)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n{categoryGroup.Key} ({categoryGroup.Count()} tests)");
        Console.ResetColor();
        Console.WriteLine(new string('-', 80));

        foreach (var tc in categoryGroup)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nID {tc.Id}: {tc.TestName}");
            Console.ResetColor();
            Console.WriteLine($"  Operation: {tc.FirstOperand} {tc.Operation} {tc.SecondOperand} = {tc.ExpectedResult}");
            if (!string.IsNullOrEmpty(tc.Description))
                Console.WriteLine($"  Description: {tc.Description}");
            Console.WriteLine($"  Created: {tc.CreatedAt:yyyy-MM-dd HH:mm:ss}");
        }
    }
}

Console.WriteLine("\n" + "=".PadRight(80, '='));
