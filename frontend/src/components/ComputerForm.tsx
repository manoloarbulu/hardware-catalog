import React, { useEffect, useRef, useState } from 'react';
import api, { Computer, ComputerType, Product, ProductCategory, WeightUnit } from '../services/api';

interface ComputerFormProps {
  initialComputer?: Computer;
  onSubmit: (computer: Omit<Computer, 'id' | 'creationDate'>) => void;
  onCancel: () => void;
}

interface SelectedProduct {
  product: Product;
  quantity: number;
}

const requiredCategories = [ProductCategory.Processor, ProductCategory.Memory, ProductCategory.Storage, ProductCategory.GraphicCard, ProductCategory.PowerSupply, ProductCategory.ExternalPorts] as const;
const multiSelectCategories = new Set<ProductCategory>([ProductCategory.Memory, ProductCategory.Storage, ProductCategory.GraphicCard, ProductCategory.ExternalPorts]);

const categoryLabels: Record<ProductCategory, string> = {
  [ProductCategory.Processor]: 'Processor',
  [ProductCategory.Memory]: 'Memory',
  [ProductCategory.Storage]: 'Hard disk',
  [ProductCategory.PowerSupply]: 'Power supply',
  [ProductCategory.ExternalPorts]: 'Port',
  [ProductCategory.GraphicCard]: 'Graphics card',
};

export const ComputerForm: React.FC<ComputerFormProps> = ({
  initialComputer,
  onSubmit,
  onCancel,
}) => {
  const [type, setType] = useState<ComputerType>(initialComputer?.type || ComputerType.Desktop);
  const [weight, setWeight] = useState<number>(initialComputer?.weight || 0);
  const [weightUnit, setWeightUnit] = useState<WeightUnit>(initialComputer?.weightUnit || WeightUnit.Kilograms);
  const [description, setDescription] = useState(initialComputer?.description || '');
  const [serialNumber, setSerialNumber] = useState(initialComputer?.serialNumber || '');
  const [manufacturer, setManufacturer] = useState(initialComputer?.manufacturer || '');
  const [catalog, setCatalog] = useState<Record<string, Product[]>>({});
  const [selected, setSelected] = useState<Record<string, SelectedProduct[]>>(() => {
    const selections: Record<string, SelectedProduct[]> = {};
    initialComputer?.products.forEach((item) => {
      if (item.product) {
        const category = item.product.category;
        selections[category] = [...(selections[category] || []), { product: item.product, quantity: item.quantity }];
      }
    });
    return selections;
  });
  const [productQueries, setProductQueries] = useState<Record<string, string>>({});
  const [openCategory, setOpenCategory] = useState<ProductCategory | null>(null);
  const [error, setError] = useState<string | null>(null);
  const activePickerRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const loadCatalog = async () => {
      try {
        const searches = await Promise.all(requiredCategories.map((category) => api.searchProducts(category)));
        setCatalog(Object.fromEntries(requiredCategories.map((category, index) => [category, searches[index].filter((product) => product.category === category)])));
      } catch {
        setError('Unable to load the component catalog.');
      }
    };

    loadCatalog();
  }, []);

  useEffect(() => {
    const closeOnOutsidePointerDown = (event: PointerEvent) => {
      if (activePickerRef.current && !activePickerRef.current.contains(event.target as Node)) {
        setOpenCategory(null);
      }
    };

    document.addEventListener('pointerdown', closeOnOutsidePointerDown);
    return () => document.removeEventListener('pointerdown', closeOnOutsidePointerDown);
  }, []);

  const selectProduct = (category: ProductCategory, productName: string) => {
    const product = catalog[category]?.find((item) => item.name === productName);
    if (product) {
      setSelected((current) => {
        const currentItems = current[category] || [];
        if (multiSelectCategories.has(category) && currentItems.some((item) => item.product.id === product.id)) {
          setError(`${product.name} is already included in this computer.`);
          return current;
        }
        const items = multiSelectCategories.has(category)
          ? [...currentItems, { product, quantity: 1 }]
          : [{ product, quantity: 1 }];
        return { ...current, [category]: items };
      });
      if (!(selected[category] || []).some((item) => item.product.id === product.id)) {
        setProductQueries((current) => ({ ...current, [category]: '' }));
        setOpenCategory(null);
        setError(null);
      }
    }
  };

  const changeQuantity = (category: ProductCategory, productId: string, quantity: number) => {
    if (quantity > 0) {
      setSelected((current) => ({
        ...current,
        [category]: (current[category] || []).map((item) => item.product.id === productId ? { ...item, quantity } : item),
      }));
    }
  };

  const removeProduct = (category: ProductCategory, productId: string) => {
    setSelected((current) => ({
      ...current,
      [category]: (current[category] || []).filter((item) => item.product.id !== productId),
    }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    if (weight <= 0) {
      setError('Weight must be greater than 0.');
      return;
    }

    const missing = requiredCategories.filter((category) => !selected[category]?.length);
    if (missing.length > 0) {
      setError(`Choose a ${missing.map((category) => categoryLabels[category]).join(', ')} before saving.`);
      return;
    }

    const computerData = {
      type,
      weight,
      weightUnit,
      description,
      serialNumber,
      manufacturer,
      products: requiredCategories.flatMap((category) => selected[category].map((item) => ({ productId: item.product.id, quantity: item.quantity }))),
    };

    onSubmit(computerData as Omit<Computer, 'id' | 'creationDate'>);
  };

  return (
    <form onSubmit={handleSubmit} className="bg-white p-6 rounded shadow-md">
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-4">
        <div>
          <label className="block font-bold mb-2">Computer Type *</label>
          <select
            value={type}
            onChange={(e) => setType(e.target.value as ComputerType)}
            className="w-full px-3 py-2 border border-gray-300 rounded"
          >
            {Object.values(ComputerType).map(t => (
              <option key={t} value={t}>{t}</option>
            ))}
          </select>
        </div>

        <div>
          <label className="block font-bold mb-2">Weight *</label>
          <div className="grid grid-cols-2 gap-2">
            <input
              type="number"
              step="0.1"
              value={weight}
              onChange={(e) => setWeight(parseFloat(e.target.value))}
              className="w-full px-3 py-2 border border-gray-300 rounded"
              required
            />
            <select
              value={weightUnit}
              onChange={(e) => setWeightUnit(e.target.value as WeightUnit)}
              className="w-full px-3 py-2 border border-gray-300 rounded"
            >
              {Object.values(WeightUnit).map(u => (
                <option key={u} value={u}>{u}</option>
              ))}
            </select>
          </div>
        </div>

        <div>
          <label className="block font-bold mb-2">Manufacturer</label>
          <input
            type="text"
            value={manufacturer}
            onChange={(e) => setManufacturer(e.target.value)}
            className="w-full px-3 py-2 border border-gray-300 rounded"
          />
        </div>

        <div>
          <label className="block font-bold mb-2">Serial Number</label>
          <input
            type="text"
            value={serialNumber}
            onChange={(e) => setSerialNumber(e.target.value)}
            className="w-full px-3 py-2 border border-gray-300 rounded"
          />
        </div>
      </div>

      <div className="mb-4">
        <label className="block font-bold mb-2">Description</label>
        <textarea
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          className="w-full px-3 py-2 border border-gray-300 rounded"
          rows={3}
        />
      </div>

      <fieldset className="mb-6">
        <legend className="font-bold mb-3">Required Components *</legend>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          {requiredCategories.map((category) => {
            const items = selected[category] || [];
            const supportsMultiple = multiSelectCategories.has(category);
            const query = productQueries[category] || '';
            const availableProducts = (catalog[category] || []).filter((product) =>
              product.name.toLowerCase().includes(query.toLowerCase()) ||
              product.brandName?.toLowerCase().includes(query.toLowerCase()) ||
              product.model.toLowerCase().includes(query.toLowerCase()));
            return <div key={category} className="border border-gray-200 p-3 rounded">
              <label className="block font-semibold">{categoryLabels[category]} *</label>
              <div ref={openCategory === category ? activePickerRef : undefined} className="relative mt-2 flex gap-2">
                <input
                  value={query}
                  placeholder={`Type to find a ${categoryLabels[category].toLowerCase()}`}
                  onFocus={() => setOpenCategory(category)}
                  onChange={(event) => {
                    const value = event.target.value;
                    setProductQueries((current) => ({ ...current, [category]: value }));
                    setOpenCategory(category);
                  }}
                  className="w-full px-3 py-2 border border-gray-300 rounded"
                  required={items.length === 0}
                />
                {openCategory === category && <div className="absolute left-0 top-full z-10 mt-1 max-h-48 w-[calc(100%-5.5rem)] overflow-y-auto border border-gray-300 bg-white shadow-lg">
                  {availableProducts.map((product) => <button key={product.id} type="button" onMouseDown={(event) => event.preventDefault()} onClick={() => selectProduct(category, product.name)} className="block w-full border-b border-gray-100 px-3 py-2 text-left text-sm hover:bg-blue-50">
                    <span className="font-medium">{product.name}</span><span className="ml-2 text-gray-600">{product.brandName} {product.model}</span>
                  </button>)}
                  {availableProducts.length === 0 && <p className="px-3 py-2 text-sm text-gray-500">No matching products.</p>}
                </div>}
                <button type="button" onClick={() => selectProduct(category, query)} className="px-3 py-2 bg-blue-500 text-white rounded hover:bg-blue-700">
                  {supportsMultiple ? 'Add' : 'Select'}
                </button>
              </div>
              {items.length > 0 && <ul className="mt-3 space-y-2 text-sm">
                {items.map((item) => <li key={item.product.id} className="flex items-center justify-between gap-2 bg-gray-50 px-2 py-2 hover:bg-blue-50">
                  <span>{item.product.name}</span>
                  <span className="whitespace-nowrap">
                    <label>Qty <input type="number" min="1" value={item.quantity} onChange={(event) => changeQuantity(category, item.product.id, Number(event.target.value))} className="ml-1 w-16 px-2 py-1 border border-gray-300 rounded" /></label>
                    <button type="button" onClick={() => removeProduct(category, item.product.id)} className="ml-2 text-red-600 hover:text-red-800">Remove</button>
                  </span>
                </li>)}
              </ul>}
            </div>;
          })}
        </div>
      </fieldset>

      {error && <p role="alert" className="mb-4 text-red-600">{error}</p>}

      <div className="flex gap-2">
        <button
          type="submit"
          className="px-4 py-2 bg-green-500 text-white rounded hover:bg-green-700"
        >
          Save Computer
        </button>
        <button
          type="button"
          onClick={onCancel}
          className="px-4 py-2 bg-gray-500 text-white rounded hover:bg-gray-700"
        >
          Cancel
        </button>
      </div>
    </form>
  );
};
