import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common'
import { AuthComponent } from './components/auth.component';
import { AuthService } from './services/auth.service';
import { AuthInterceptor } from './services/auth.interceptor';

@NgModule({
    imports: [CommonModule, ReactiveFormsModule, HttpClientModule],
    declarations: [AuthComponent],
    exports: [AuthComponent],
    providers: [AuthService, {
        provide: HTTP_INTERCEPTORS,
        useClass: AuthInterceptor,
        multi: true
    }]
})
export class AuthModule { }