export type ProductCategory =
  | 'bread'
  | 'pastries'
  | 'cupcakes'
  | 'chocolates'
  | 'truffles'
  | 'cakes';

export interface Product {
  id: number;
  name: string;
  category: ProductCategory;
  price: number;
  image: string;
  description: string;
}

export interface CartItem extends Product {
  quantity: number;
}

export const PRODUCTS: Product[] = [
  // Bread
  {
    id: 1,
    name: 'Sourdough Boule',
    category: 'bread',
    price: 8.99,
    image: 'https://images.unsplash.com/photo-1627308593341-d886acdc06a2?w=600&q=80',
    description: 'Traditional sourdough with a crispy crust and tangy flavor',
  },
  {
    id: 2,
    name: 'Artisan Baguette',
    category: 'bread',
    price: 4.50,
    image: 'https://images.unsplash.com/photo-1549931319-a545dcf3bc73?w=600&q=80',
    description: 'Classic French baguette, crusty outside, soft inside',
  },
  {
    id: 3,
    name: 'Multigrain Loaf',
    category: 'bread',
    price: 7.50,
    image: 'https://images.unsplash.com/photo-1586444248902-2f64eddc13df?w=600&q=80',
    description: 'Hearty bread packed with seeds and whole grains',
  },
  // Pastries
  {
    id: 4,
    name: 'Butter Croissant',
    category: 'pastries',
    price: 3.50,
    image: 'https://images.unsplash.com/photo-1555507036-ab1f4038808a?w=600&q=80',
    description: 'Flaky, buttery croissant made with premium French butter',
  },
  {
    id: 5,
    name: 'Almond Croissant',
    category: 'pastries',
    price: 4.50,
    image: 'https://images.unsplash.com/photo-1623334044303-241021148842?w=600&q=80',
    description: 'Croissant filled with almond cream and topped with sliced almonds',
  },
  {
    id: 6,
    name: 'Danish Pastry',
    category: 'pastries',
    price: 4.00,
    image: 'https://images.unsplash.com/photo-1558303485-4f1a8cd0b6d1?w=600&q=80',
    description: 'Light, flaky pastry with seasonal fruit filling',
  },
  {
    id: 7,
    name: 'Pain au Chocolat',
    category: 'pastries',
    price: 4.00,
    image: 'https://images.unsplash.com/photo-1737700089128-cbbb2dc71631?w=600&q=80',
    description: 'Buttery pastry with rich dark chocolate',
  },
  // Cupcakes
  {
    id: 8,
    name: 'Vanilla Bean Cupcake',
    category: 'cupcakes',
    price: 4.50,
    image: 'https://images.unsplash.com/photo-1761746350777-b86ae72e8f41?w=600&q=80',
    description: 'Classic vanilla cupcake with creamy buttercream frosting',
  },
  {
    id: 9,
    name: 'Red Velvet Cupcake',
    category: 'cupcakes',
    price: 5.00,
    image: 'https://images.unsplash.com/photo-1607101985529-4f8d2b27c98b?w=600&q=80',
    description: 'Moist red velvet with cream cheese frosting',
  },
  {
    id: 10,
    name: 'Chocolate Fudge Cupcake',
    category: 'cupcakes',
    price: 5.00,
    image: 'https://images.unsplash.com/photo-1481391319762-47dff72954d9?w=600&q=80',
    description: 'Rich chocolate cupcake with chocolate ganache',
  },
  {
    id: 11,
    name: 'Lemon Lavender Cupcake',
    category: 'cupcakes',
    price: 5.50,
    image: 'https://images.unsplash.com/photo-1499636136210-6f4ee915583e?w=600&q=80',
    description: 'Light lemon cupcake with lavender buttercream',
  },
  // Chocolates
  {
    id: 12,
    name: 'Dark Chocolate Bar',
    category: 'chocolates',
    price: 8.50,
    image: 'https://images.unsplash.com/photo-1569622701449-32fe4e90e492?w=600&q=80',
    description: '70% single-origin dark chocolate',
  },
  {
    id: 13,
    name: 'Milk Chocolate Bar',
    category: 'chocolates',
    price: 7.50,
    image: 'https://images.unsplash.com/photo-1481391319762-47dff72954d9?w=600&q=80',
    description: 'Creamy milk chocolate with Madagascar vanilla',
  },
  {
    id: 14,
    name: 'Sea Salt Caramel Bar',
    category: 'chocolates',
    price: 9.00,
    image: 'https://images.unsplash.com/photo-1558326567-98ae2405596b?w=600&q=80',
    description: 'Dark chocolate with sea salt caramel filling',
  },
  // Truffles
  {
    id: 15,
    name: 'Classic Truffle Box',
    category: 'truffles',
    price: 18.00,
    image: 'https://images.unsplash.com/photo-1695649912699-435a5bc20203?w=600&q=80',
    description: 'Box of 6 assorted chocolate truffles',
  },
  {
    id: 16,
    name: 'Champagne Truffle Box',
    category: 'truffles',
    price: 22.00,
    image: 'https://images.unsplash.com/photo-1488477181946-6428a0291777?w=600&q=80',
    description: 'Box of 6 champagne-infused truffles',
  },
  {
    id: 17,
    name: 'Raspberry Truffle Box',
    category: 'truffles',
    price: 20.00,
    image: 'https://images.unsplash.com/photo-1519915028121-7d3463d20b13?w=600&q=80',
    description: 'Box of 6 raspberry dark chocolate truffles',
  },
  // Cakes
  {
    id: 18,
    name: 'Chocolate Layer Cake',
    category: 'cakes',
    price: 45.00,
    image: 'https://images.unsplash.com/photo-1578985545062-69928b1d9587?w=600&q=80',
    description: 'Three layers of rich chocolate cake with ganache (8")',
  },
  {
    id: 19,
    name: 'Vanilla Celebration Cake',
    category: 'cakes',
    price: 42.00,
    image: 'https://images.unsplash.com/photo-1464349095431-e9a21285b5f3?w=600&q=80',
    description: 'Classic vanilla cake with buttercream frosting (8")',
  },
  {
    id: 20,
    name: 'Strawberry Shortcake',
    category: 'cakes',
    price: 38.00,
    image: 'https://images.unsplash.com/photo-1737700088028-fae0666feb83?w=600&q=80',
    description: 'Light sponge cake with fresh strawberries and cream (8")',
  },
  {
    id: 21,
    name: 'Lemon Drizzle Cake',
    category: 'cakes',
    price: 35.00,
    image: 'https://images.unsplash.com/photo-1486427944299-d1955d23e34d?w=600&q=80',
    description: 'Moist lemon cake with tangy lemon glaze (8")',
  },
];
