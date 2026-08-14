import { useEffect, useState } from 'react';
import { useComputers } from './hooks/useComputers';
import { ComputerList } from './components/ComputerList';
import { ComputerForm } from './components/ComputerForm';
import { ProductSearch } from './components/ProductSearch';
import { ProductMaintenance } from './components/ProductMaintenance';
import { Computer, Product } from './services/api';
import './index.css';

type View = 'list' | 'form' | 'search' | 'products';

function App() {
  const { computers, loading, fetchComputers, addComputer, updateComputer, deleteComputer } = useComputers();
  const [view, setView] = useState<View>('list');
  const [editingComputer, setEditingComputer] = useState<Computer | null>(null);
  const [selectedProducts, setSelectedProducts] = useState<Product[]>([]);

  useEffect(() => {
    fetchComputers();
  }, []);

  const handleEditComputer = (computer: Computer) => {
    setEditingComputer(computer);
    setView('form');
  };

  const handleDeleteComputer = async (id: string) => {
    if (window.confirm('Are you sure you want to delete this computer?')) {
      await deleteComputer(id);
    }
  };

  const handleSaveComputer = async (data: Omit<Computer, 'id' | 'creationDate'>) => {
    if (editingComputer) {
      await updateComputer(editingComputer.id, data);
    } else {
      await addComputer(data);
    }
    setView('list');
    setEditingComputer(null);
    await fetchComputers();
  };

  const handleProductSelect = (product: Product) => {
    if (!selectedProducts.find(p => p.id === product.id)) {
      setSelectedProducts([...selectedProducts, product]);
    }
  };

  return (
    <div className="min-h-screen bg-gray-100">
      <nav className="bg-blue-600 text-white p-4">
        <div className="container mx-auto">
          <h1 className="text-3xl font-bold mb-4">Hardware Catalog</h1>
          <div className="flex gap-4">
            <button
              onClick={() => { setView('list'); setEditingComputer(null); }}
              className={`px-4 py-2 rounded ${view === 'list' ? 'bg-blue-800' : 'bg-blue-500 hover:bg-blue-700'}`}
            >
              Computers
            </button>
            <button
              onClick={() => { setView('form'); setEditingComputer(null); }}
              className={`px-4 py-2 rounded ${view === 'form' ? 'bg-blue-800' : 'bg-blue-500 hover:bg-blue-700'}`}
            >
              Add Computer
            </button>
            <button
              onClick={() => setView('products')}
              className={`px-4 py-2 rounded ${view === 'products' ? 'bg-blue-800' : 'bg-blue-500 hover:bg-blue-700'}`}
            >
              Maintain Products
            </button>
            <button
              onClick={() => setView('search')}
              className={`px-4 py-2 rounded ${view === 'search' ? 'bg-blue-800' : 'bg-blue-500 hover:bg-blue-700'}`}
            >
              Search Products
            </button>
          </div>
        </div>
      </nav>

      <div className="container mx-auto p-4">
        {view === 'list' && (
          <div>
            <h2 className="text-2xl font-bold mb-4">Computer List</h2>
            <ComputerList
              computers={computers}
              loading={loading}
              onEdit={handleEditComputer}
              onDelete={handleDeleteComputer}
            />
          </div>
        )}

        {view === 'form' && (
          <div>
            <h2 className="text-2xl font-bold mb-4">
              {editingComputer ? 'Edit Computer' : 'Create New Computer'}
            </h2>
            <ComputerForm
              initialComputer={editingComputer || undefined}
              onSubmit={handleSaveComputer}
              onCancel={() => { setView('list'); setEditingComputer(null); }}
            />
          </div>
        )}

        {view === 'search' && (
          <ProductSearch onProductSelect={handleProductSelect} />
        )}

        {view === 'products' && <ProductMaintenance />}
      </div>
    </div>
  );
}

export default App;
