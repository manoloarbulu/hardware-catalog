# AI Infrastructure Guide

Your hardware catalog repository now has a complete AI customization framework for GitHub Copilot and custom agents.

## Directory Structure

```
hardware-catalog/
├── .instructions.md           # Root-level custom instructions
├── .agent.md                  # Agent behavior configuration
├── copilot-instructions.md    # Copilot-specific settings
├── AGENTS.md                  # Agent registry and documentation
│
├── .vscode/
│   └── settings.json          # VS Code workspace settings
│
├── .copilot/
│   └── config.json            # Copilot configuration
│
├── skills/                    # Custom AI skills directory
│   └── README.md              # Skills documentation
│   └── [your-skills]/
│       └── SKILL.md           # Skill definitions
│
├── agents/                    # Custom agents directory
│   └── README.md              # Agents documentation
│   └── [your-agents]/
│       └── [agent-name].md    # Agent configurations
│
└── prompts/                   # Custom prompts directory
    └── README.md              # Prompts documentation
    └── [categories]/
        └── [prompt-name].md   # Prompt templates
```

## Quick Start

### 1. Customize Instructions

Edit these files to teach Copilot about your hardware catalog:

- **[.instructions.md](./.instructions.md)** - General project context and conventions
- **[copilot-instructions.md](./copilot-instructions.md)** - Copilot-specific behavior

### 2. Create Custom Skills

Add domain expertise in `skills/`:

```bash
skills/
├── hardware-specifications/
│   └── SKILL.md
├── inventory-management/
│   └── SKILL.md
└── vendor-integration/
    └── SKILL.md
```

See [skills/README.md](./skills/README.md) for detailed instructions.

### 3. Define Custom Agents

Create specialized agents in `agents/`:

```bash
agents/
├── catalog-manager.md
├── inventory-specialist.md
└── data-validator.md
```

Register them in [AGENTS.md](./AGENTS.md).

### 4. Build Prompt Templates

Store reusable prompts in `prompts/`:

```bash
prompts/
├── catalog-operations/
│   ├── new-entry.md
│   └── update-specs.md
├── data-validation/
│   └── quality-check.md
└── documentation/
    └── generate-readme.md
```

### 5. Configure VS Code

Workspace settings are in `.vscode/settings.json` for:

- Code formatting preferences
- File associations
- Editor behavior
- Copilot integration

## File Purposes

### Configuration Files

| File                      | Purpose                | When to Edit                    |
| ------------------------- | ---------------------- | ------------------------------- |
| `.instructions.md`        | Global project context | When defining project standards |
| `copilot-instructions.md` | Copilot behavior       | When tuning AI responses        |
| `.agent.md`               | Agent defaults         | When setting agent behavior     |
| `AGENTS.md`               | Agent registry         | When adding new agents          |

### Directories

| Directory   | Purpose                 | When to Use                      |
| ----------- | ----------------------- | -------------------------------- |
| `skills/`   | Reusable expertise      | For specialized domain knowledge |
| `agents/`   | Specialized AI personas | For task-specific workflows      |
| `prompts/`  | Template interactions   | For consistent workflows         |
| `.vscode/`  | Editor configuration    | For development environment      |
| `.copilot/` | AI configuration        | For AI behavior tuning           |

## Common Tasks

### Add a New Skill

1. Create `skills/[skill-name]/SKILL.md`
2. Follow the template in [skills/README.md](./skills/README.md)
3. Reference in relevant agents

### Create a Custom Agent

1. Create `agents/[agent-name].md`
2. Document in [AGENTS.md](./AGENTS.md)
3. Link related skills

### Save Favorite Prompts

1. Create category folder in `prompts/[category]/`
2. Add `[prompt-name].md` files
3. Reference when working with Copilot

### Update Project Context

Edit `.instructions.md` with:

- Hardware catalog specifics
- Data structures
- Integration requirements
- Naming conventions
- Quality standards

## AI Capabilities Enabled

With this structure, you can:

✅ **Customize Copilot** - Teach it about your domain  
✅ **Create Agents** - Build specialized AI workflows  
✅ **Define Skills** - Package domain expertise  
✅ **Store Prompts** - Reuse effective interactions  
✅ **Configure Environment** - Optimize development settings

## Integration Points

```
.instructions.md
       ↓
   Copilot  ←→  copilot-instructions.md
       ↓
   .agent.md  ←→  AGENTS.md
       ↓
   agents/  ←→  skills/
       ↓
    prompts/
```

## Best Practices

1. **Keep Instructions Concise** - Use bullet points and examples
2. **Document Skills Thoroughly** - Include use cases and patterns
3. **Version Control** - Commit all AI configuration files
4. **Test Prompts** - Refine based on results
5. **Update Regularly** - As your project evolves

## File Templates

### New Skill Template

Create `skills/[name]/SKILL.md`:

```markdown
---
name: skill-name
description: What this skill covers
tags: [category, subtopic]
---

# Skill Name

## When to Use

Specific scenarios...

## Key Concepts

1. Concept 1
2. Concept 2

## Example

[Code or usage example]
```

### New Agent Template

Create `agents/[name].md`:

```markdown
# [Agent Name]

## Expertise

- Area 1
- Area 2

## Skills Used

- Skill 1
- Skill 2

## Special Behaviors

1. Behavior 1
2. Behavior 2
```

### New Prompt Template

Create `prompts/[category]/[name].md`:

```markdown
# [Prompt Name]

**Use Case**: What this is for

## Prompt

[Your actual prompt]

## Tips

- Tip 1
- Tip 2
```

## Next Steps

1. ✏️ Update [.instructions.md](./.instructions.md) with your project details
2. 🛠️ Create your first skill in [skills/](./skills/)
3. 🤖 Define custom agents in [agents/](./agents/)
4. 📝 Add prompt templates to [prompts/](./prompts/)
5. 🔧 Customize [.vscode/settings.json](./.vscode/settings.json) as needed

## References

- [Skills Documentation](./skills/README.md)
- [Agents Documentation](./agents/README.md)
- [Prompts Documentation](./prompts/README.md)
- [Copilot Configuration](./copilot-instructions.md)
- [GitHub Copilot Documentation](https://github.com/features/copilot)

## Support

For questions about:

- **Skills** → See [skills/README.md](./skills/README.md)
- **Agents** → See [agents/README.md](./agents/README.md) and [AGENTS.md](./AGENTS.md)
- **Prompts** → See [prompts/README.md](./prompts/README.md)
- **Configuration** → See `.vscode/settings.json` and `.copilot/config.json`
