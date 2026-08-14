# Prompts Directory

Custom prompt templates for the hardware catalog.

## What are Prompts?

Prompts are:

- Reusable question and instruction templates
- Optimized for specific tasks or workflows
- Examples of how to interact with Copilot effectively
- Guidelines for consistent results

## Directory Structure

```
prompts/
├── README.md (this file)
├── [prompt-category]/
│   └── [prompt-name].md
```

## Prompt Template

Create a prompt file with:

```markdown
# [Prompt Name]

**Category**: [Category]
**Use Case**: What this prompt is for
**Complexity**: Simple / Intermediate / Advanced

## Prompt Text

[Your actual prompt here]

## Expected Output

Description of what you should expect back.

## Tips

- Tip 1
- Tip 2

## Example
```

[Example of using this prompt]

```

## Variations

- Variation 1: How to modify for different needs
- Variation 2: Alternative approaches
```

## Categories

Suggested prompt categories for hardware catalog:

- **Catalog Operations** - Creating and modifying catalog entries
- **Data Validation** - Checking data quality and consistency
- **Inventory Management** - Inventory-related queries
- **Documentation** - Writing documentation and specs
- **Analysis** - Analyzing hardware data and relationships
- **Integration** - Working with external systems
- **Troubleshooting** - Debugging and fixing issues

## Example Prompts

### Catalog Entry Creation

```
Use the Catalog Manager Agent to create a new hardware catalog entry for:
- Product: [Product Name]
- Category: [Category]
- Key Specifications: [List specs]
Include validation and documentation.
```

### Data Quality Check

```
Analyze the hardware catalog for:
1. Missing required fields
2. Inconsistent naming conventions
3. Invalid specification ranges
Provide a detailed report with fixes.
```

## Best Practices

1. **Be Specific**: Include details about your hardware
2. **Define Constraints**: Mention any limitations
3. **Specify Output Format**: Explain how you want results
4. **Provide Context**: Reference existing catalog structures
5. **Request Validation**: Ask for data verification

## Using Prompts with Copilot

Save your favorite prompts and use them repeatedly:

```
I have a prompt saved at: prompts/catalog-operations/new-entry.md
Can you use this to help me...?
```

## See Also

- [Skills](../skills/) - Domain knowledge for better prompts
- [Agents](../agents/) - Specialized agents for complex tasks
- [AGENTS.md](../AGENTS.md) - Agent capabilities
