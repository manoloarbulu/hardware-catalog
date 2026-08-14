import React, { useState } from 'react';
import { useProductSearch } from '../hooks/useProductSearch';
import { Product } from '../services/api';

interface ProductSearchProps {
  onProductSelect: (product: Product) => void;
}

export const ProductSearch: React.FC<ProductSearchProps> = ({ onProductSelect }) => {
  const { results, loading, search } = useProductSearch();
  const [query, setQuery] = useState('');

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault();
    search(query);
  };

  return (
    <div className="bg-white p-6 rounded shadow-md">
      <h2 className="text-2xl font-bold mb-4">Search Products</h2>

      <form onSubmit={handleSearch} className="mb-4">
        <input
          type="text"
          placeholder="Search products (e.g., 'Show me 1TB storage drives')"
          value={query}
          onChange={(e) => setQuery(e.target.value)}
          className="w-full px-4 py-2 border border-gray-300 rounded mb-2"
        />
        <button
          type="submit"
          className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-700"
        >
          Search
        </button>
      </form>

      {loading && <p>Searching...</p>}

      {results.length > 0 && (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {results.map(product => (
            <div key={product.id} className="p-4 border border-gray-300 rounded hover:bg-blue-50 hover:shadow-lg">
              <h3 className="font-bold">{product.name}</h3>
              <p className="text-sm text-gray-600">{product.brandName} - {product.model}</p>
              <p className="text-sm text-gray-600">Category: {product.category}</p>
              <p className="text-sm text-gray-600">Unit: {product.unitOfMeasure}</p>
              <button
                onClick={() => onProductSelect(product)}
                className="mt-2 px-3 py-1 bg-green-500 text-white rounded hover:bg-green-700 w-full"
              >
                Add to Computer
              </button>
            </div>
          ))}
        </div>
      )}

      {!loading && results.length === 0 && query && (
        <p className="text-gray-600">No products found.</p>
      )}
    </div>
  );
};
