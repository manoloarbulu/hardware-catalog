import React from 'react';
import { Computer, ProductCategory } from '../services/api';

interface ComputerListProps {
  computers: Computer[];
  loading: boolean;
  onEdit: (computer: Computer) => void;
  onDelete: (id: string) => void;
}

export const ComputerList: React.FC<ComputerListProps> = ({ computers, loading, onEdit, onDelete }) => {
  if (loading) {
    return <div className="p-4">Loading...</div>;
  }

  const formatProducts = (computer: Computer) => {
    const grouped = computer.products.reduce<Record<string, string[]>>((groups, item) => {
      const category = item.product?.category ?? 'Other';
      groups[category] = [...(groups[category] || []), `${item.product?.name ?? 'Unknown product'} x${item.quantity}`];
      return groups;
    }, {});
    const categories = Object.values(ProductCategory);
    const summary = categories.filter((category) => grouped[category]).map((category) => `${category}: ${grouped[category].join(', ')}`);
    return [summary.slice(0, 3).join(' | '), summary.slice(3).join(' | ')].filter(Boolean);
  };

  if (computers.length === 0) {
    return <div className="p-4">No computers found.</div>;
  }

  return (
    <div className="overflow-x-auto">
      <table className="min-w-full bg-white border border-gray-300">
        <thead className="bg-gray-200">
          <tr>
            <th className="px-4 py-2 border">Type</th>
            <th className="px-4 py-2 border whitespace-nowrap">Weight</th>
            <th className="px-4 py-2 border">Created</th>
            <th className="px-4 py-2 border">Products</th>
            <th className="px-4 py-2 border whitespace-nowrap">Actions</th>
          </tr>
        </thead>
        <tbody>
          {computers.map(computer => (
            <tr key={computer.id} className="border-b hover:bg-gray-50">
              <td className="px-4 py-2 border">{computer.type}</td>
              <td className="px-4 py-2 border whitespace-nowrap">{computer.weight} {computer.weightUnit}</td>
              <td className="px-4 py-2 border">{new Date(computer.creationDate).toLocaleDateString()}</td>
              <td className="px-4 py-2 border text-xs leading-5">
                {formatProducts(computer).map((line) => <p key={line} className="truncate" title={line}>{line}</p>)}
              </td>
              <td className="px-4 py-2 border whitespace-nowrap">
                <button
                  onClick={() => onEdit(computer)}
                  className="mr-2 px-3 py-1 bg-blue-500 text-white rounded hover:bg-blue-700"
                >
                  Edit
                </button>
                <button
                  onClick={() => onDelete(computer.id)}
                  className="px-3 py-1 bg-red-500 text-white rounded hover:bg-red-700"
                >
                  Delete
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};
