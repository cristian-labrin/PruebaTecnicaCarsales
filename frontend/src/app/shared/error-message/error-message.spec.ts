import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ErrorMessage } from './error-message';

describe('ErrorMessage', () => {
  let component: ErrorMessage;
  let fixture: ComponentFixture<ErrorMessage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ErrorMessage],
    }).compileComponents();

    fixture = TestBed.createComponent(ErrorMessage);
    component = fixture.componentInstance;
  });

  it('debería crearse', () => {
    expect(component).toBeTruthy();
  });

  it('debería mostrar el mensaje recibido', () => {
    fixture.componentRef.setInput('message', 'Algo salió mal');
    fixture.detectChanges();

    const text = fixture.nativeElement.querySelector('.error__text');
    expect(text.textContent).toContain('Algo salió mal');
  });

  it('no debería mostrar el botón de reintentar por defecto', () => {
    fixture.detectChanges();

    const button = fixture.nativeElement.querySelector('.error__btn');
    expect(button).toBeNull();
  });

  it('debería mostrar el botón de reintentar cuando showRetry es true', () => {
    fixture.componentRef.setInput('showRetry', true);
    fixture.detectChanges();

    const button = fixture.nativeElement.querySelector('.error__btn');
    expect(button).not.toBeNull();
  });

  it('debería emitir el evento retry al hacer click en reintentar', () => {
    fixture.componentRef.setInput('showRetry', true);
    fixture.detectChanges();

    let emitted = false;
    component.retry.subscribe(() => (emitted = true));

    const button = fixture.nativeElement.querySelector('.error__btn') as HTMLButtonElement;
    button.click();

    expect(emitted).toBe(true);
  });
});
