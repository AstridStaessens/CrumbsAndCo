import { Component } from '@angular/core';

interface GalleryImage {
  id: number;
  src: string;
  alt: string;
  caption: string;
  tall?: boolean;
}

@Component({
  selector: 'app-galerij',
  standalone: true,
  imports: [],
  templateUrl: './galerij.component.html',
  styleUrl: './galerij.component.scss',
})
export class GalerijComponent {
  images: GalleryImage[] = [
    { id: 1, src: 'https://images.unsplash.com/photo-1578985545062-69928b1d9587?w=600&q=80', alt: 'Chocoladetaart', caption: 'Chocolade ganache taart', tall: true },
    { id: 2, src: 'https://images.unsplash.com/photo-1464349095431-e9a21285b5f3?w=600&q=80', alt: 'Verjaardagstaart', caption: 'Verjaardagstaart op maat' },
    { id: 3, src: 'https://images.unsplash.com/photo-1488477181946-6428a0291777?w=600&q=80', alt: 'Fruitentaart', caption: 'Seizoensfruitentaart' },
    { id: 4, src: 'https://images.unsplash.com/photo-1519915028121-7d3463d20b13?w=600&q=80', alt: 'Citroentaart', caption: 'Citroentaart met meringue' },
    { id: 5, src: 'https://images.unsplash.com/photo-1558326567-98ae2405596b?w=600&q=80', alt: 'Macarons', caption: 'Macarons seizoenssmaken', tall: true },
    { id: 6, src: 'https://images.unsplash.com/photo-1499636136210-6f4ee915583e?w=600&q=80', alt: 'Koeken', caption: 'Koeken assortiment' },
    { id: 7, src: 'https://images.unsplash.com/photo-1607101985529-4f8d2b27c98b?w=600&q=80', alt: 'Brownies', caption: 'Brownies met walnoten' },
    { id: 8, src: 'https://images.unsplash.com/photo-1486427944299-d1955d23e34d?w=600&q=80', alt: 'Bloemendecoratie', caption: 'Bloemendecoratie taart' },
    { id: 9, src: 'https://images.unsplash.com/photo-1621303837174-89787a7d4729?w=600&q=80', alt: 'Layercake', caption: 'Layercake confetti' },
    { id: 10, src: 'https://images.unsplash.com/photo-1627308593341-d886acdc06a2?w=600&q=80', alt: 'Brood', caption: 'Artisanaal brood', tall: true },
    { id: 11, src: 'https://images.unsplash.com/photo-1555507036-ab1f4038808a?w=600&q=80', alt: 'Croissants', caption: 'Verse croissants' },
    { id: 12, src: 'https://images.unsplash.com/photo-1695649912699-435a5bc20203?w=600&q=80', alt: 'Truffels', caption: 'Handgemaakte truffels' },
  ];
}
