# Day 1.3: Security Best Practices with GitHub Copilot

## Objective

Learn security considerations when using GitHub Copilot:
- Protecting sensitive data in prompts
- Secure code generation verification
- API key and credential management
- Privacy and compliance considerations

## What You'll Learn

1. What NOT to include in Copilot prompts
2. How to verify security of generated code
3. Best practices for handling sensitive operations
4. Compliance and data privacy considerations

## Critical Security Rules

### ❌ DO NOT Include in Copilot Prompts:

- API keys, tokens, or credentials
- Database connection strings
- Personal Identifiable Information (PII)
- Financial information
- Health records or sensitive data
- Password hashes or cryptographic keys
- Proprietary business logic or trade secrets

### ✅ DO Include in Promilot Prompts:

- Placeholder variable names: `apiKey`, `dbConnectionString`
- Business logic description without sensitive data
- Security requirements and constraints
- Compliance frameworks (GDPR, HIPAA, SOC 2)
- Security patterns and best practices

## Key Demonstration Files

- `secure-credential-handling.cs` - Proper credential management patterns
- `secure-api-design.py` - API security best practices
- `security-patterns.cs` - Common security patterns

## Running the Demonstrations

```powershell
# Review and run security patterns
python secure-api-design.py
dotnet run secure-credential-handling.cs
```

## Post-Generation Security Checklist

- [ ] No sensitive data leaked in logs or errors
- [ ] Input validation implemented
- [ ] Output encoding applied
- [ ] Authentication/authorization checks present
- [ ] SQL injection prevention (parameterized queries)
- [ ] CORS and CSRF protections configured
- [ ] Rate limiting implemented
- [ ] Error messages don't expose internals
- [ ] Dependencies checked for vulnerabilities
- [ ] Code reviewed by security expert
