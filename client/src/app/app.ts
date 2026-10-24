import { Component, inject, signal } from '@angular/core';
import { User } from './users/user';
import { UsersService } from './users/users.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
})
export class App {
  private readonly usersService = inject(UsersService);

  protected readonly title = 'The Dating App';
  protected readonly users = signal<User[]>([]);
  protected readonly loading = signal(true);
  protected readonly error = signal<string | null>(null);

  constructor() {
    this.usersService.getUsers().subscribe({
      next: (users) => {
        this.users.set(users);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Could not load users. Is the API running?');
        this.loading.set(false);
      },
    });
  }
}
