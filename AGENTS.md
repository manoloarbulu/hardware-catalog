# Agents Registry

This file documents all custom agents available for the hardware catalog repository.

## Agent Types

### Built-in Agents

1. **Explore Agent**
   - Fast read-only codebase exploration
   - Code search and Q&A
   - Use for: Finding code, understanding structure, answering questions

2. **Default Agent**
   - General coding and debugging
   - File management and editing
   - Code generation and refactoring

### Custom Agents

Define your custom agents below. Each agent can have specialized skills and instructions.

```markdown
## [Agent Name]

**Purpose**: Brief description of what this agent does

**Expertise**: List areas of specialization

**Tools**: Specific tools this agent uses

**Example Usage**: How to invoke this agent

**Related Skills**: Links to skills this agent uses
```

## Adding New Agents

To create a new agent:

1. Define agent behavior in [.agent.md](./.agent.md)
2. Create agent-specific configuration in [agents/](./agents/)
3. Document in this registry
4. Link related skills from [skills/](./skills/)

## Integration with Skills

Agents leverage skills for domain-specific knowledge:

- See [skills/](./skills/) for available skills
- Each skill provides specialized expertise
- Agents can combine multiple skills
