"""
Day 2.6 Demo: Simple MCP (Model Context Protocol) Server
Demonstrates basic MCP server architecture and patterns.
"""

import asyncio
import json
from typing import Any, Dict, List, Optional, Callable
from enum import Enum
from dataclasses import dataclass, asdict
from abc import ABC, abstractmethod


class MessageType(Enum):
    """MCP message types"""
    INITIALIZE = "initialize"
    LIST_TOOLS = "tools/list"
    CALL_TOOL = "tools/call"
    LIST_RESOURCES = "resources/list"
    GET_RESOURCE = "resources/get"


@dataclass
class Tool:
    """MCP Tool definition"""
    name: str
    description: str
    input_schema: Dict[str, Any]


@dataclass
class Resource:
    """MCP Resource definition"""
    uri: str
    name: str
    description: str
    mime_type: str = "text/plain"


class MCPServer(ABC):
    """Base class for MCP Server implementations"""
    
    def __init__(self, name: str, version: str):
        self.name = name
        self.version = version
        self.tools: Dict[str, Tool] = {}
        self.resources: Dict[str, Resource] = {}
        self.tool_handlers: Dict[str, Callable] = {}
        self.resource_handlers: Dict[str, Callable] = {}
    
    def register_tool(self, tool: Tool, handler: Callable) -> None:
        """Register a tool that Copilot can call"""
        self.tools[tool.name] = tool
        self.tool_handlers[tool.name] = handler
    
    def register_resource(self, resource: Resource, handler: Callable) -> None:
        """Register a resource that Copilot can access"""
        self.resources[resource.uri] = resource
        self.resource_handlers[resource.uri] = handler
    
    async def handle_message(self, message: Dict[str, Any]) -> Dict[str, Any]:
        """Route incoming MCP messages"""
        message_type = message.get("type")
        
        if message_type == MessageType.INITIALIZE.value:
            return await self.handle_initialize(message)
        elif message_type == MessageType.LIST_TOOLS.value:
            return await self.handle_list_tools()
        elif message_type == MessageType.CALL_TOOL.value:
            return await self.handle_call_tool(message)
        elif message_type == MessageType.LIST_RESOURCES.value:
            return await self.handle_list_resources()
        elif message_type == MessageType.GET_RESOURCE.value:
            return await self.handle_get_resource(message)
        else:
            return {"error": f"Unknown message type: {message_type}"}
    
    async def handle_initialize(self, message: Dict[str, Any]) -> Dict[str, Any]:
        """Handle initialization message"""
        return {
            "type": "initialize_response",
            "server": {
                "name": self.name,
                "version": self.version
            }
        }
    
    async def handle_list_tools(self) -> Dict[str, Any]:
        """List all available tools"""
        return {
            "type": "tools_response",
            "tools": [asdict(tool) for tool in self.tools.values()]
        }
    
    async def handle_call_tool(self, message: Dict[str, Any]) -> Dict[str, Any]:
        """Call a tool handler"""
        tool_name = message.get("tool_name")
        arguments = message.get("arguments", {})
        
        if tool_name not in self.tool_handlers:
            return {"error": f"Tool not found: {tool_name}"}
        
        try:
            handler = self.tool_handlers[tool_name]
            result = await handler(**arguments)
            return {
                "type": "tool_response",
                "content": result
            }
        except Exception as e:
            return {"error": str(e)}
    
    async def handle_list_resources(self) -> Dict[str, Any]:
        """List all available resources"""
        return {
            "type": "resources_response",
            "resources": [asdict(res) for res in self.resources.values()]
        }
    
    async def handle_get_resource(self, message: Dict[str, Any]) -> Dict[str, Any]:
        """Get a specific resource"""
        resource_uri = message.get("uri")
        
        if resource_uri not in self.resource_handlers:
            return {"error": f"Resource not found: {resource_uri}"}
        
        try:
            handler = self.resource_handlers[resource_uri]
            content = await handler()
            return {
                "type": "resource_response",
                "uri": resource_uri,
                "content": content
            }
        except Exception as e:
            return {"error": str(e)}


class DatabaseMCPServer(MCPServer):
    """Example MCP Server for database operations"""
    
    def __init__(self):
        super().__init__("database-mcp-server", "1.0.0")
        self._setup_tools()
        self._setup_resources()
    
    def _setup_tools(self) -> None:
        """Configure database tools"""
        
        # Query tool
        query_tool = Tool(
            name="execute_query",
            description="Execute SQL query against the database",
            input_schema={
                "type": "object",
                "properties": {
                    "sql": {
                        "type": "string",
                        "description": "SQL query to execute"
                    },
                    "params": {
                        "type": "array",
                        "description": "Query parameters"
                    }
                },
                "required": ["sql"]
            }
        )
        self.register_tool(query_tool, self._execute_query_handler)
        
        # Optimization tool
        optimize_tool = Tool(
            name="optimize_query",
            description="Analyze and optimize SQL query performance",
            input_schema={
                "type": "object",
                "properties": {
                    "sql": {
                        "type": "string",
                        "description": "SQL query to optimize"
                    }
                },
                "required": ["sql"]
            }
        )
        self.register_tool(optimize_tool, self._optimize_query_handler)
    
    def _setup_resources(self) -> None:
        """Configure database resources"""
        
        # Schema resource
        schema_resource = Resource(
            uri="database://schema",
            name="Database Schema",
            description="Current database schema and table definitions",
            mime_type="text/plain"
        )
        self.register_resource(schema_resource, self._get_schema_handler)
        
        # Best practices resource
        practices_resource = Resource(
            uri="database://best-practices",
            name="Database Best Practices",
            description="Best practices for query optimization and database design",
            mime_type="text/markdown"
        )
        self.register_resource(practices_resource, self._get_practices_handler)
    
    async def _execute_query_handler(self, sql: str, params: Optional[List] = None) -> Dict:
        """Handle query execution"""
        # In real implementation, connect to actual database
        return {
            "success": True,
            "rows_affected": 0,
            "message": f"Query executed: {sql[:50]}..."
        }
    
    async def _optimize_query_handler(self, sql: str) -> Dict:
        """Handle query optimization"""
        suggestions = []
        
        if "SELECT *" in sql:
            suggestions.append("Use specific columns instead of SELECT *")
        
        if "JOIN" in sql and "INDEX" not in sql:
            suggestions.append("Ensure proper indexes on JOIN columns")
        
        if sql.count("LEFT JOIN") > 2:
            suggestions.append("Consider query restructuring with multiple JOINs")
        
        return {
            "original_query": sql,
            "suggestions": suggestions,
            "potential_improvement": "15-30% performance improvement"
        }
    
    async def _get_schema_handler(self) -> str:
        """Return database schema information"""
        return """
# Database Schema

## Tables

### Users
- id (INT, PRIMARY KEY)
- username (VARCHAR(100))
- email (VARCHAR(100))
- created_at (DATETIME)

### Products
- id (INT, PRIMARY KEY)
- name (VARCHAR(200))
- price (DECIMAL)
- stock (INT)
- category_id (INT, FK)

### Orders
- id (INT, PRIMARY KEY)
- user_id (INT, FK)
- product_id (INT, FK)
- quantity (INT)
- order_date (DATETIME)

## Indexes
- Users.email (UNIQUE)
- Products.category_id
- Orders.user_id
- Orders.order_date
"""
    
    async def _get_practices_handler(self) -> str:
        """Return database best practices"""
        return """
# Database Best Practices

## Query Optimization
1. Use specific columns, avoid SELECT *
2. Add WHERE clauses to filter data early
3. Create indexes on frequently filtered columns
4. Use EXPLAIN PLAN to analyze queries
5. Batch multiple operations when possible

## Schema Design
1. Normalize to 3NF for OLTP systems
2. Use appropriate data types
3. Add meaningful constraints
4. Document all columns and relationships
5. Plan for growth and scalability

## Performance Tips
1. Use parameterized queries (prevents SQL injection)
2. Connection pooling for multiple requests
3. Cache frequently accessed data
4. Archive old data periodically
5. Monitor query performance regularly
"""


# Demonstration
async def main():
    print("=" * 70)
    print("MCP (Model Context Protocol) Server Demonstration")
    print("=" * 70)
    
    # Create server
    server = DatabaseMCPServer()
    
    print("\n1. Initialize Server:")
    init_msg = await server.handle_message({
        "type": "initialize"
    })
    print(json.dumps(init_msg, indent=2))
    
    print("\n2. List Available Tools:")
    tools_msg = await server.handle_message({
        "type": "tools/list"
    })
    print(json.dumps(tools_msg, indent=2))
    
    print("\n3. List Available Resources:")
    resources_msg = await server.handle_message({
        "type": "resources/list"
    })
    print(json.dumps(resources_msg, indent=2))
    
    print("\n4. Call Tool - Optimize Query:")
    optimize_msg = await server.handle_message({
        "type": "tools/call",
        "tool_name": "optimize_query",
        "arguments": {
            "sql": "SELECT * FROM products LEFT JOIN categories ON products.category_id = categories.id LEFT JOIN suppliers ON categories.supplier_id = suppliers.id"
        }
    })
    print(json.dumps(optimize_msg, indent=2))
    
    print("\n5. Get Resource - Database Schema:")
    schema_msg = await server.handle_message({
        "type": "resources/get",
        "uri": "database://schema"
    })
    print(json.dumps(schema_msg, indent=2))
    
    print("\n6. Get Resource - Best Practices:")
    practices_msg = await server.handle_message({
        "type": "resources/get",
        "uri": "database://best-practices"
    })
    print(json.dumps(practices_msg, indent=2))
    
    print("\n" + "=" * 70)
    print("Demonstration Complete!")
    print("=" * 70)


if __name__ == "__main__":
    asyncio.run(main())
