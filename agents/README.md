# Agents Directory

Specialized AI agents for hardware catalog operations.

## What are Agents?

Agents are specialized AI personas that:

- Handle specific types of tasks
- Apply specialized knowledge
- Combine multiple skills
- Maintain consistent behavior patterns

## Directory Structure

```
agents/
├── README.md (this file)
├── [agent-name].md (agent definition)
```

## Creating a Custom Agent

Create an agent configuration file with:

```markdown
# [Agent Name]

## Persona

Brief description of this agent's personality and expertise.

## Expertise Areas

- Area 1
- Area 2
- Area 3

## Skills Used

- Skill 1
- Skill 2

## Tools Allowed

- Tool 1
- Tool 2

## Special Behaviors

1. Behavior 1
2. Behavior 2

## Example Invocation

How users would invoke this agent.

## Related Documentation

Links to relevant files.
```

## Example Agents

Placeholder for hardware catalog agents:

- **Catalog Manager Agent** - Manages catalog structure and updates
- **Inventory Specialist** - Handles inventory operations
- **Specification Expert** - Works with hardware specifications
- **Data Validator** - Ensures data quality and integrity

## Invoking Agents

To use a custom agent:

```
@agent-name [your request]
```

Or reference by skill:

```
@copilot use the [Agent Name] agent to...
```

## Agent Lifecycle

1. **Definition**: Create agent configuration in this directory
2. **Registration**: Document in [../AGENTS.md](../AGENTS.md)
3. **Validation**: Test with sample tasks
4. **Documentation**: Add examples and use cases

## See Also

- [Skills](../skills/) - Knowledge modules agents can use
- [AGENTS.md](../AGENTS.md) - Agent registry and documentation
- [Prompts](../prompts/) - Custom prompts agents can follow
