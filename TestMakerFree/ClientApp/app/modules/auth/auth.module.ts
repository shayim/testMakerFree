import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common'
import { AuthComponent } from './components/auth.component';
import { AuthService } from './services/auth.service';

@NgModule({
    imports: [CommonModule, ReactiveFormsModule, HttpClientModule],
    declarations: [AuthComponent],
    exports: [AuthComponent],
    providers: [AuthService]
})
export class AuthModule { }