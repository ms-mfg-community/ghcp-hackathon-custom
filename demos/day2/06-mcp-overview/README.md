# Day 2.6: Model Context Protocol (MCP) Overview

## Objective

Introduction to Model Context Protocol and custom integrations:
- MCP architecture and concepts
- Building custom MCP servers
- Integration patterns with GitHub Copilot
- Use cases and benefits
- Advanced context injection

## What is MCP?

Model Context Protocol (MCP) is an open protocol for connecting AI models like Claude with external tools, APIs, and data sources. It enables:

- **Custom Tools** - Add domain-specific capabilities
- **Knowledge Bases** - Inject project-specific context
- **API Integration** - Connect to internal services
- **Real-time Data** - Access live information
- **Secure Connections** - Authenticated external access

## MCP Architecture

```
┌─────────────────┐
│  GitHub Copilot │
└────────┬────────┘
         │ (MCP Protocol)
┌────────▼────────────────────────────────────┐
│    MCP Transport Layer                      │
│  (stdio, HTTP, WebSocket, etc.)             │
└────────┬────────────────────────────────────┘
         │
┌────────▼────────────────────────────────────┐
│    Custom MCP Server                        │
│  ┌──────────────────────────────────────┐  │
│  │  Tool Handlers                       │  │
│  │  - Database Queries                  │  │
│  │  - API Calls                         │  │
│  │  - File Operations                   │  │
│  │  - Custom Logic                      │  │
│  └──────────────────────────────────────┘  │
│  ┌──────────────────────────────────────┐  │
│  │  Resource Handlers                   │  │
│  │  - Knowledge Base                    │  │
│  │  - Documentation                     │  │
│  │  - Schemas                           │  │
│  └──────────────────────────────────────┘  │
└────────┬────────────────────────────────────┘
         │
      ┌──┴──┬──────┬────────┐
      │     │      │        │
    ┌─▼─┐ ┌┴──┐ ┌─▼──┐ ┌──▼──┐
    │DB │ │API│ │App │ │Tool │
    └───┘ └───┘ └────┘ └─────┘
```

## Key Components

### 1. Tools
Functions that AI models can call:
- Take input parameters
- Perform actions
- Return results
- Have descriptions for Copilot

### 2. Resources
Context data for the AI:
- Project documentation
- Code schemas
- Knowledge bases
- Configuration

### 3. Prompts
Pre-configured instructions:
- Domain-specific workflows
- Best practices
- Standard patterns

## Example Use Cases

| Use Case | Benefit |
|----------|---------|
| **Code Review** | Custom linting rules, security patterns |
| **Database** | Schema information, query optimization tips |
| **DevOps** | Infrastructure state, deployment history |
| **Testing** | Test frameworks, fixtures, mocking patterns |
| **Documentation** | Auto-link to docs, examples, API specs |

## Demonstration Files

- `mcp-server-example.py` - Basic MCP server implementation
- `mcp-client-integration.py` - Integration patterns
- `mcp-use-cases.md` - Real-world examples

## Building Custom MCP Server

### Step 1: Define Tools

```python
TOOLS = [
    {
        "name": "query_database",
        "description": "Execute SQL queries on the project database",
        "inputSchema": {
            "type": "object",
            "properties": {
                "sql": {"type": "string"}
            },
            "required": ["sql"]
        }
    }
]
```

### Step 2: Implement Handlers

```python
async def query_database(sql: str):
    # Execute query
    # Return results
```

### Step 3: Register with Copilot

Configuration in VS Code settings:

```json
{
  "github.copilot.customServers": [
    {
      "name": "database-mcp",
      "command": "python mcp-server.py"
    }
  ]
}
```

## Best Practices

1. **Clear Descriptions** - Help Copilot understand tool purpose
2. **Error Handling** - Return meaningful error messages
3. **Performance** - Optimize tool responses (<2s)
4. **Security** - Validate inputs, authenticate access
5. **Versioning** - Maintain backward compatibility
6. **Documentation** - Include examples and edge cases
7. **Testing** - Test tools independently

## Resources

- [MCP Documentation](https://modelcontextprotocol.io)
- [GitHub Copilot Integration Docs](https://docs.github.com/en/copilot)
- [Custom Integration Patterns](https://github.blog/ai-and-ml/)
