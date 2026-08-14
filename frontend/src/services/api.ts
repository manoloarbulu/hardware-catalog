import axios, { AxiosInstance } from 'axios';

export enum ProductCategory {
  Memory = 'Memory',
  Storage = 'Storage',
  ExternalPorts = 'ExternalPorts',
  GraphicCard = 'GraphicCard',
  PowerSupply = 'PowerSupply',
  Processor = 'Processor',
}

export enum UnitOfMeasure {
  MB = 'MB',
  GB = 'GB',
  TB = 'TB',
  Units = 'Units',
  Watts = 'Watts',
  Voltage = 'Voltage',
}

export enum WeightUnit {
  Pounds = 'Pounds',
  Kilograms = 'Kilograms',
  Grams = 'Grams',
}

export enum ComputerType {
  Laptop = 'Laptop',
  Desktop = 'Desktop',
  Server = 'Server',
  BladeServer = 'BladeServer',
}

export interface Product {
  id: string;
  category: ProductCategory;
  name: string;
  unitOfMeasure: UnitOfMeasure;
  value: number;
  brandId: string;
  model: string;
  brandName?: string;
}

export interface Brand {
  id: string;
  name: string;
}

export type ProductInput = Omit<Product, 'id' | 'brandName'>;

export interface ComputerProduct {
  productId: string;
  quantity: number;
  product?: Product;
}

export interface Computer {
  id: string;
  creationDate: string;
  type: ComputerType;
  weight: number;
  weightUnit: WeightUnit;
  description?: string;
  serialNumber?: string;
  manufactureDate?: string;
  manufacturer?: string;
  products: ComputerProduct[];
}

class HardwareCatalogApi {
  private client: AxiosInstance;

  constructor(baseURL: string = '/api') {
    this.client = axios.create({
      baseURL,
      headers: {
        'Content-Type': 'application/json',
      },
    });
  }

  // Computers API
  async getAllComputers(): Promise<Computer[]> {
    const response = await this.client.get<Computer[]>('/computers');
    return response.data;
  }

  async getComputerById(id: string): Promise<Computer | null> {
    try {
      const response = await this.client.get<Computer>(`/computers/${id}`);
      return response.data;
    } catch {
      return null;
    }
  }

  async createComputer(computer: Omit<Computer, 'id' | 'creationDate'>): Promise<Computer> {
    const response = await this.client.post<Computer>('/computers', computer);
    return response.data;
  }

  async updateComputer(id: string, computer: Omit<Computer, 'id' | 'creationDate'>): Promise<Computer> {
    const response = await this.client.put<Computer>(`/computers/${id}`, {
      ...computer,
      id,
    });
    return response.data;
  }

  async deleteComputer(id: string): Promise<boolean> {
    try {
      await this.client.delete(`/computers/${id}`);
      return true;
    } catch {
      return false;
    }
  }

  // Products API
  async searchProducts(query: string): Promise<Product[]> {
    const response = await this.client.get<Product[]>('/products/search', {
      params: { query },
    });
    return response.data;
  }

  async getProducts(category?: ProductCategory, brandId?: string): Promise<Product[]> {
    const response = await this.client.get<Product[]>('/products', { params: { category, brandId } });
    return response.data;
  }

  async getBrands(): Promise<Brand[]> {
    const response = await this.client.get<Brand[]>('/brands');
    return response.data;
  }

  async createProduct(product: ProductInput): Promise<Product> {
    const response = await this.client.post<Product>('/products', product);
    return response.data;
  }

  async updateProduct(id: string, product: ProductInput): Promise<Product> {
    const response = await this.client.put<Product>(`/products/${id}`, product);
    return response.data;
  }

  async deleteProduct(id: string): Promise<void> {
    await this.client.delete(`/products/${id}`);
  }
}

export default new HardwareCatalogApi();
