import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-error-message',
  standalone: true,
  templateUrl: './error-message.html',
  styleUrl: './error-message.css'
})
export class ErrorMessage {
  readonly message = input<string>('Ocurrió un error inesperado.');
  readonly showRetry = input<boolean>(false);
  readonly retry = output<void>();

  protected onRetry(): void {
    this.retry.emit();
  }
}
