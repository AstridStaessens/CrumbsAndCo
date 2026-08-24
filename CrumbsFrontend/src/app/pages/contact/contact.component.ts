import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ContactService } from '../../services/contact.service';

@Component({
  selector: 'app-contact',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './contact.component.html',
  styleUrl: './contact.component.scss',
})
export class ContactComponent {
  private contactService = inject(ContactService);

  form = { naam: '', email: '', bericht: '' };
  submitted = false;
  isLoading = false;
  error = '';

  onSubmit(): void {
    this.isLoading = true;
    this.error = '';

    this.contactService.sendContact(this.form).subscribe({
      next: () => {
        this.submitted = true;
        this.isLoading = false;
        this.form = { naam: '', email: '', bericht: '' };
      },
      error: () => {
        this.error = 'Bericht kon niet verzonden worden. Probeer het later opnieuw.';
        this.isLoading = false;
      },
    });
  }
}
