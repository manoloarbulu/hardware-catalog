import { useState, useCallback } from 'react';
import api, { Computer } from '../services/api';

interface UseComputerReturn {
  computers: Computer[];
  loading: boolean;
  error: string | null;
  fetchComputers: () => Promise<void>;
  addComputer: (computer: Omit<Computer, 'id' | 'creationDate'>) => Promise<Computer | null>;
  updateComputer: (id: string, computer: Omit<Computer, 'id' | 'creationDate'>) => Promise<Computer | null>;
  deleteComputer: (id: string) => Promise<boolean>;
}

export const useComputers = (): UseComputerReturn => {
  const [computers, setComputers] = useState<Computer[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchComputers = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await api.getAllComputers();
      setComputers(data);
    } catch (err) {
      setError('Failed to fetch computers');
      console.error(err);
    } finally {
      setLoading(false);
    }
  }, []);

  const addComputer = useCallback(async (computer: Omit<Computer, 'id' | 'creationDate'>) => {
    try {
      const newComputer = await api.createComputer(computer);
      setComputers([...computers, newComputer]);
      return newComputer;
    } catch (err) {
      setError('Failed to create computer');
      console.error(err);
      return null;
    }
  }, [computers]);

  const updateComputerFn = useCallback(async (id: string, computer: Omit<Computer, 'id' | 'creationDate'>) => {
    try {
      const updated = await api.updateComputer(id, computer);
      setComputers(computers.map(c => c.id === id ? updated : c));
      return updated;
    } catch (err) {
      setError('Failed to update computer');
      console.error(err);
      return null;
    }
  }, [computers]);

  const deleteComputerFn = useCallback(async (id: string) => {
    try {
      const success = await api.deleteComputer(id);
      if (success) {
        setComputers(computers.filter(c => c.id !== id));
      }
      return success;
    } catch (err) {
      setError('Failed to delete computer');
      console.error(err);
      return false;
    }
  }, [computers]);

  return {
    computers,
    loading,
    error,
    fetchComputers,
    addComputer,
    updateComputer: updateComputerFn,
    deleteComputer: deleteComputerFn,
  };
};
