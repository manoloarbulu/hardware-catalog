# Skill: API Communication with Axios

**Pattern Definition:**

1. Create a centralized Axios instance in `src/services/apiClient.ts`.
2. Configure a base URL pointing to the .NET API (e.g., `http://localhost:5000/api`).
3. Add a response interceptor to globally handle 400 and 500 level errors via standard `console.error` or toast notifications.
4. Export strongly-typed async functions for UI components to consume.

**Example Snippet:**

```typescript
import axios from "axios";

export const apiClient = axios.create({
  baseURL: "http://localhost:5000/api",
  timeout: 5000,
});

export const getComputers = async (): Promise<Computer[]> => {
  const { data } = await apiClient.get<Computer[]>("/computers");
  return data;
};
```
