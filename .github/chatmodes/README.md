# GitHub Copilot Custom Chat Modes

This directory contains custom chat mode definitions for GitHub Copilot. Custom chat modes allow you to tailor Copilot's behavior for specific domains or workflows.

## Available Chat Modes

### 🏗️ Azure Principal Architect
**File:** `azure-principal-architect.chatmode.md`

Expert Azure architecture guidance using Azure Well-Architected Framework (WAF) principles and Microsoft best practices.

**Use this mode for:**
- Azure architecture design and review
- Well-Architected Framework assessments
- Azure service selection and configuration
- Azure security, reliability, and performance optimization
- Azure cost optimization strategies

**Key features:**
- Evaluates designs against all 5 WAF pillars (Security, Reliability, Performance, Cost, Operational Excellence)
- Searches Microsoft Learn and Azure Architecture Center for latest guidance
- Provides specific Azure service recommendations with reference architectures
- Explicit trade-off discussions between architectural pillars

---

### 💻 Beast Mode 4.1
**File:** `beast-mode-4.1.chatmode.md`

High-performance autonomous coding agent with extensive internet research capabilities.

**Use this mode for:**
- Complex problem-solving requiring autonomous iteration
- Tasks requiring extensive internet research and documentation review
- Multi-step solutions that need validation and testing
- Problems requiring thorough edge case handling

**Key features:**
- Autonomous operation until problem is fully solved
- Extensive web research using fetch tools
- Step-by-step planning with markdown todo lists
- Rigorous testing and validation
- Debugging and iteration until perfection

---

### ☁️ Cloud Agent
**File:** `cloud-agent.chatmode.md`

Expert multi-cloud platform agent providing architecture guidance, Infrastructure as Code (IaC) implementation, and DevOps best practices across Azure, AWS, and GCP.

**Use this mode for:**
- Multi-cloud architecture design
- Infrastructure as Code (Terraform, Bicep, CloudFormation, Pulumi)
- Cloud platform selection and migration
- DevOps and CI/CD pipeline implementation
- Cloud security and compliance
- Cost optimization across cloud platforms
- Container orchestration and serverless architectures
- Cloud-native application development

**Key features:**
- Multi-cloud expertise (Azure, AWS, GCP)
- Infrastructure as Code code generation and best practices
- Platform-specific service recommendations with equivalency mappings
- Security, cost optimization, and operational guidance
- Complete working code examples with production-ready configurations
- Research-backed recommendations from official cloud provider documentation

---

## How to Use Custom Chat Modes

### In GitHub Copilot Chat (VS Code)

1. Open the Copilot Chat panel (Ctrl+Alt+I or Cmd+Alt+I)
2. Type `@workspace /mode` to see available modes
3. Select the desired chat mode from the list
4. Start your conversation - Copilot will follow the mode's instructions

### Switching Modes

You can switch between modes at any time during your conversation:
- Use the mode selector in the chat interface
- Or type `@workspace /mode <mode-name>`

### Creating Your Own Custom Chat Modes

Custom chat modes are defined using markdown files with YAML frontmatter:

```markdown
---
description: 'Brief description of what this mode does'
tools: ['list', 'of', 'tools', 'this', 'mode', 'can', 'use']
---

# Your Mode Instructions

Detailed instructions for how Copilot should behave in this mode...
```

**Required frontmatter fields:**
- `description`: Brief description shown in the mode selector
- `tools`: Array of tool names the mode can access

**Common tools:**
- `changes` - View workspace changes
- `codebase` - Search and understand the codebase
- `editFiles` - Edit files in the workspace
- `fetch` - Fetch web content for research
- `githubRepo` - Interact with GitHub repositories
- `runCommands` - Execute shell commands
- `runTests` - Run test suites
- `search` - Search workspace
- `vscodeAPI` - Access VS Code APIs

## Best Practices

1. **Choose the right mode** - Select the mode that best matches your task domain
2. **Provide context** - Give the agent information about your requirements, constraints, and goals
3. **Be specific** - Clear requirements lead to better results
4. **Validate output** - Always review and test generated code
5. **Iterate** - Use multi-turn conversations to refine solutions

## Contributing

To add a new custom chat mode:

1. Create a new `.chatmode.md` file in this directory
2. Include required YAML frontmatter (`description` and `tools`)
3. Write clear instructions for Copilot's behavior
4. Test the mode with various scenarios
5. Update this README with your mode's description and use cases

## Additional Resources

- [GitHub Copilot Documentation](https://docs.github.com/copilot)
- [Azure Well-Architected Framework](https://learn.microsoft.com/azure/well-architected/)
- [AWS Well-Architected Framework](https://aws.amazon.com/architecture/well-architected/)
- [Google Cloud Architecture Framework](https://cloud.google.com/architecture/framework)
- [Terraform Documentation](https://www.terraform.io/docs)
- [Azure Bicep Documentation](https://learn.microsoft.com/azure/azure-resource-manager/bicep/)

---

**Last Updated:** November 2025
