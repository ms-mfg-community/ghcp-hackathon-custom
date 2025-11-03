# Day 1.2: GitHub Copilot Flow & Workflows

## Objective

Master multi-turn conversations and workflows with GitHub Copilot:
- Context management across multiple turns
- Iterative refinement of code suggestions
- Using chat for complex problem-solving
- Building larger solutions through dialog

## What You'll Learn

1. How to structure multi-turn conversations effectively
2. Providing feedback to refine Copilot suggestions
3. Using Copilot Chat for architecture and design discussions
4. Breaking down complex problems into manageable steps

## Key Concepts

### Multi-Turn Conversation Pattern

1. **Initial Request**: Describe the overall goal
2. **Clarification**: Ask Copilot for implementation approach
3. **Refinement**: Request adjustments based on feedback
4. **Verification**: Ask Copilot to explain the solution
5. **Optimization**: Request improvements or alternatives

### Effective Context Provision

- Include existing code patterns in the chat
- Reference specific files or classes
- Provide requirements and constraints
- Mention performance or security considerations

## Example Workflow Files

- `conversation-flow.md` - Example multi-turn conversation
- `workflow-example-1.py` - Complex solution built with Copilot Flow
- `workflow-example-2.cs` - .NET solution using iterative refinement

## Running the Demonstrations

```powershell
# Review the conversation flow example
cat conversation-flow.md

# Run Python workflow example
python workflow-example-1.py

# Build and run C# workflow example
dotnet new console -n WorkflowDemo
copy workflow-example-2.cs WorkflowDemo/Program.cs
cd WorkflowDemo
dotnet run
```

## Best Practices for Multi-Turn Conversations

1. ✅ Start with a clear, high-level goal
2. ✅ Ask questions about implementation approach
3. ✅ Request step-by-step breakdown for complex tasks
4. ✅ Use feedback rounds to refine suggestions
5. ✅ Ask for explanations to verify understanding
6. ❌ Don't assume the first suggestion is optimal
7. ❌ Don't provide too much context at once
8. ❌ Don't skip testing intermediate suggestions
