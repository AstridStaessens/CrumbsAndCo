import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ContactService } from '../../services/contact.service';

interface OpMaatForm {
  naam: string;
  email: string;
  telefoon: string;
  type: string;
  datum: string;
  wensen: string;
  fileName: string;
}

@Component({
  selector: 'app-op-maat',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './op-maat.component.html',
  styleUrl: './op-maat.component.scss',
})
export class OpMaatComponent {
  private contactService = inject(ContactService);

  form: OpMaatForm = {
    naam: '',
    email: '',
    telefoon: '',
    type: '',
    datum: '',
    wensen: '',
    fileName: '',
  };

  submitted = false;
  isLoading = false;
  error = '';

  onFileChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.form.fileName = input.files[0].name;
    }
  }

  onSubmit(): void {
    this.isLoading = true;
    this.error = '';

    this.contactService.sendCustomOrder(this.form).subscribe({
      next: () => {
        this.submitted = true;
        this.isLoading = false;
      },
      error: () => {
        this.error = 'Aanvraag kon niet verzonden worden. Probeer het later opnieuw.';
        this.isLoading = false;
      },
    });
  }

  resetForm(): void {
    this.form = { naam: '', email: '', telefoon: '', type: '', datum: '', wensen: '', fileName: '' };
    this.submitted = false;
    this.error = '';
  }
}
