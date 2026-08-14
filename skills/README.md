# Skills Directory

Domain-specific knowledge packages for the hardware catalog.

## What are Skills?

Skills are specialized knowledge modules that provide:

- Detailed instructions for specific tasks
- Domain expertise and best practices
- Tool usage guidance
- Integration patterns

## Directory Structure

```
skills/
├── README.md (this file)
├── [skill-name]/
│   ├── SKILL.md (skill definition)
│   └── [supporting files]
```

## Creating a New Skill

Each skill should have a `SKILL.md` file with:

```yaml
---
name: skill-name
description: Brief description of this skill
requires: [optional list of prerequisites]
tags: [category, tags]
---

# Skill Name

## Overview
Detailed description of what this skill covers.

## When to Use
Specific scenarios where this skill applies.

## Prerequisites
What you need to know or have before using this skill.

## Key Concepts
1. Concept 1
2. Concept 2
3. Concept 3

## Examples
Practical examples of using this skill.

## Common Patterns
Patterns and best practices.

## References
Links to related documentation.
```

## Example Skills

Placeholder for skills related to hardware catalog:

- **Hardware Specifications** - Understanding and validating hardware specs
- **Catalog Schema** - Working with the catalog data structure
- **Inventory Management** - Tracking and updating inventory
- **Vendor Integration** - Integrating with vendor systems
- **Compatibility Checking** - Validating hardware compatibility
- **Data Validation** - Ensuring data integrity

## Using Skills with Copilot

Copilot can reference and apply skills from this directory. To invoke a skill:

```
@copilot Use the [skill-name] skill to help with...
```

## See Also

- [Agents](../agents/) - Specialized agents that use skills
- [Prompts](../prompts/) - Custom prompt templates
- [.instructions.md](../.instructions.md) - Root-level instructions
