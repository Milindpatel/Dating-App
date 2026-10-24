import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { environment } from '../environments/environment';
import { App } from './app';

describe('App', () => {
  let http: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [App],
      providers: [provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  it('lists users from the API', async () => {
    const fixture = TestBed.createComponent(App);
    http.expectOne(`${environment.apiUrl}users`).flush([
      { id: 1, userName: 'bob' },
      { id: 2, userName: 'tom' },
    ]);
    await fixture.whenStable();
    const items = (fixture.nativeElement as HTMLElement).querySelectorAll('li');
    expect(Array.from(items).map((li) => li.textContent?.trim())).toEqual(['1 - bob', '2 - tom']);
  });

  it('shows an error when the API is down', async () => {
    const fixture = TestBed.createComponent(App);
    http.expectOne(`${environment.apiUrl}users`).flush('down', { status: 0, statusText: 'Unknown Error' });
    await fixture.whenStable();
    expect((fixture.nativeElement as HTMLElement).querySelector('.alert')?.textContent).toContain('Could not load users');
  });
});
