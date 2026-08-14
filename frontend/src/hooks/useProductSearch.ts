import { useState, useCallback } from 'react';
import api, { Product } from '../services/api';

interface UseProductSearchReturn {
  results: Product[];
  loading: boolean;
  error: string | null;
  search: (query: string) => Promise<void>;
}

export const useProductSearch = (): UseProductSearchReturn => {
  const [results, setResults] = useState<Product[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const search = useCallback(async (query: string) => {
    if (!query.trim()) {
      setResults([]);
      return;
    }

    setLoading(true);
    setError(null);
    try {
      const data = await api.searchProducts(query);
      setResults(data);
    } catch (err) {
      setError('Failed to search products');
      console.error(err);
    } finally {
      setLoading(false);
    }
  }, []);

  return { results, loading, error, search };
};
