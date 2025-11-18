namespace calculator.tests.Data;

/// <summary>
/// Collection definition to disable parallel test execution for CRUD operations tests
/// This ensures tests don't interfere with each other when using shared database context
/// </summary>
[CollectionDefinition("CrudOperationsTests", DisableParallelization = true)]
public class CrudOperationsTestCollection
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
