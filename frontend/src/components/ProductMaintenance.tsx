import { useEffect, useRef, useState } from 'react';
import api, { Brand, Product, ProductCategory, ProductInput, UnitOfMeasure } from '../services/api';

const emptyProduct = (): ProductInput => ({ name: '', model: '', category: ProductCategory.Processor, unitOfMeasure: UnitOfMeasure.Units, brandId: '' });

export const ProductMaintenance = () => {
  const [brands, setBrands] = useState<Brand[]>([]);
  const [products, setProducts] = useState<Product[]>([]);
  const [category, setCategory] = useState<ProductCategory | null>(null);
  const [brandId, setBrandId] = useState<string | null>(null);
  const [categoryBrands, setCategoryBrands] = useState<Brand[]>([]);
  const [filterPicker, setFilterPicker] = useState<'category' | 'brand' | null>(null);
  const [editing, setEditing] = useState<Product | null>(null);
  const [formOpen, setFormOpen] = useState(false);
  const [form, setForm] = useState<ProductInput>(emptyProduct());
  const [message, setMessage] = useState<string | null>(null);
  const pickerRef = useRef<HTMLDivElement>(null);

  useEffect(() => { api.getBrands().then(setBrands).catch(() => setMessage('Unable to load brands.')); }, []);
  useEffect(() => {
    if (category || brandId) api.getProducts(category ?? undefined, brandId ?? undefined).then(setProducts).catch(() => setMessage('Unable to load products.'));
    else setProducts([]);
  }, [category, brandId]);
  useEffect(() => {
    if (!category) {
      setCategoryBrands([]);
      return;
    }

    api.getProducts(category).then((categoryProducts) => {
      const availableBrandIds = new Set(categoryProducts.map((product) => product.brandId));
      const availableBrands = brands.filter((brand) => availableBrandIds.has(brand.id));
      setCategoryBrands(availableBrands);
      if (brandId && !availableBrandIds.has(brandId)) setBrandId(null);
    }).catch(() => setMessage('Unable to load brands for the selected category.'));
  }, [category, brands]);
  useEffect(() => {
    const close = (event: PointerEvent) => { if (pickerRef.current && !pickerRef.current.contains(event.target as Node)) setFilterPicker(null); };
    document.addEventListener('pointerdown', close);
    return () => document.removeEventListener('pointerdown', close);
  }, []);

  const openCreate = () => { setEditing(null); setFormOpen(true); setForm({ ...emptyProduct(), category: category ?? ProductCategory.Processor, brandId: brandId ?? brands[0]?.id ?? '' }); setMessage(null); };
  const openEdit = (product: Product) => { setEditing(product); setFormOpen(true); setForm({ name: product.name, model: product.model, category: product.category, unitOfMeasure: product.unitOfMeasure, brandId: product.brandId }); setMessage(null); };
  const save = async (event: React.FormEvent) => {
    event.preventDefault();
    try {
      const saved = editing ? await api.updateProduct(editing.id, form) : await api.createProduct(form);
      setProducts((current) => editing ? current.map((product) => product.id === saved.id ? saved : product) : [...current, saved]);
      setEditing(null); setFormOpen(false); setForm(emptyProduct()); setMessage('Product saved.');
    } catch { setMessage('Unable to save product. Verify all fields are complete.'); }
  };
  const remove = async (product: Product) => {
    if (!window.confirm(`Delete ${product.name}?`)) return;
    try { await api.deleteProduct(product.id); setProducts((current) => current.filter((item) => item.id !== product.id)); setMessage('Product deleted.'); }
    catch (error: any) { setMessage(error.response?.data?.error ?? 'This product cannot be deleted.'); }
  };
  const clearSelectedProduct = () => {
    setEditing(null);
    setFormOpen(false);
    setForm(emptyProduct());
    setMessage(null);
  };
  const selectCategory = (value: ProductCategory) => { clearSelectedProduct(); setCategory(value); setFilterPicker(null); };
  const selectBrand = (value: string) => { clearSelectedProduct(); setBrandId(value); setFilterPicker(null); };
  const filterBrands = category ? categoryBrands : brands;

  return <div className="space-y-5">
    <div className="flex items-center justify-between"><h2 className="text-2xl font-bold">Product Maintenance</h2><button onClick={openCreate} className="px-4 py-2 bg-green-600 text-white rounded hover:bg-green-700">Add Product</button></div>
    <div ref={pickerRef} className="grid grid-cols-1 gap-4 md:grid-cols-2">
      <div className="relative"><label className="block font-semibold">Filter by Category</label><button type="button" onClick={() => setFilterPicker(filterPicker === 'category' ? null : 'category')} className="mt-2 w-full border border-gray-300 bg-white px-3 py-2 text-left rounded">{category ?? 'Select a category'}</button>
        {filterPicker === 'category' && <div className="absolute z-10 mt-1 w-full border border-gray-300 bg-white shadow-lg">{Object.values(ProductCategory).map((item) => <button type="button" key={item} onClick={() => selectCategory(item)} className="block w-full px-3 py-2 text-left hover:bg-blue-50">{item}</button>)}</div>}</div>
      <div className="relative"><label className="block font-semibold">Filter by Brand</label><button type="button" onClick={() => setFilterPicker(filterPicker === 'brand' ? null : 'brand')} className="mt-2 w-full border border-gray-300 bg-white px-3 py-2 text-left rounded">{brands.find((brand) => brand.id === brandId)?.name ?? 'Select a brand'}</button>
        {filterPicker === 'brand' && <div className="absolute z-10 mt-1 max-h-52 w-full overflow-y-auto border border-gray-300 bg-white shadow-lg">{filterBrands.map((brand) => <button type="button" key={brand.id} onClick={() => selectBrand(brand.id)} className="block w-full px-3 py-2 text-left hover:bg-blue-50">{brand.name}</button>)}{filterBrands.length === 0 && <p className="px-3 py-2 text-sm text-gray-500">No brands in this category.</p>}</div>}</div>
    </div>
    {formOpen && <form onSubmit={save} className="grid grid-cols-1 gap-3 border border-gray-300 bg-white p-4 md:grid-cols-2">
      <h3 className="md:col-span-2 text-lg font-bold">{editing ? 'Edit Product' : 'Add Product'}</h3>
      <label className="block font-semibold">Product Name
        <input value={form.name} onChange={(event) => setForm({ ...form, name: event.target.value })} className="mt-1 w-full border border-gray-300 px-3 py-2 rounded" required />
      </label>
      <label className="block font-semibold">Model
        <input value={form.model} onChange={(event) => setForm({ ...form, model: event.target.value })} className="mt-1 w-full border border-gray-300 px-3 py-2 rounded" required />
      </label>
      <label className="block font-semibold">Category
        <select value={form.category} onChange={(event) => setForm({ ...form, category: event.target.value as ProductCategory })} disabled={Boolean(editing)} className="mt-1 w-full border border-gray-300 px-3 py-2 rounded disabled:bg-gray-100 disabled:text-gray-500">{Object.values(ProductCategory).map((item) => <option key={item}>{item}</option>)}</select>
      </label>
      <label className="block font-semibold">Unit of Measure
        <select value={form.unitOfMeasure} onChange={(event) => setForm({ ...form, unitOfMeasure: event.target.value as UnitOfMeasure })} className="mt-1 w-full border border-gray-300 px-3 py-2 rounded">{Object.values(UnitOfMeasure).map((item) => <option key={item}>{item}</option>)}</select>
      </label>
      <label className="block font-semibold">Brand
        <select value={form.brandId} onChange={(event) => setForm({ ...form, brandId: event.target.value })} className="mt-1 w-full border border-gray-300 px-3 py-2 rounded" required><option value="">Select a brand</option>{brands.map((brand) => <option key={brand.id} value={brand.id}>{brand.name}</option>)}</select>
      </label>
      <div className="flex items-end gap-2"><button type="submit" className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700">Save</button><button type="button" onClick={() => { setEditing(null); setFormOpen(false); setForm(emptyProduct()); }} className="px-4 py-2 bg-gray-500 text-white rounded">Cancel</button></div>
    </form>}
    {message && <p role="status" className="text-sm text-gray-700">{message}</p>}
    {!category && !brandId ? <p className="text-gray-600">Select a category, brand, or both to list products.</p> : <div className="overflow-x-auto bg-white"><table className="min-w-full border border-gray-300"><thead className="bg-gray-200"><tr><th className="px-3 py-2 text-left">Name</th><th className="px-3 py-2 text-left">Category</th><th className="px-3 py-2 text-left">Brand</th><th className="px-3 py-2 text-left">Model</th><th className="px-3 py-2">Actions</th></tr></thead><tbody>{products.map((product) => <tr key={product.id} className="border-t hover:bg-blue-50"><td className="px-3 py-2">{product.name}</td><td className="px-3 py-2">{product.category}</td><td className="px-3 py-2">{product.brandName}</td><td className="px-3 py-2">{product.model}</td><td className="px-3 py-2 text-center"><button onClick={() => openEdit(product)} className="mr-2 text-blue-700 hover:text-blue-900">Edit</button><button onClick={() => remove(product)} className="text-red-700 hover:text-red-900">Delete</button></td></tr>)}</tbody></table></div>}
  </div>;
};
