import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
    selector: 'app-auth',
    templateUrl: 'auth.component.html',
    styleUrls: ['auth.component.css']
})
export class AuthComponent implements OnInit {
    loginForm: FormGroup | undefined;
    constructor(private authService: AuthService, private fb: FormBuilder, private router: Router) { }

    ngOnInit() {
        this.loginForm = this.fb.group({
            username: ['', Validators.required],
            password: ['', Validators.required]
        });
    }

    login() {
        this.authService.login(this.loginForm!.value)
            .subscribe(data => this.router.navigate(['/home']),
                error => console.log(error));
    }

    get usernameCtl() {
        return this.loginForm!.get('username');
    }

    get passwordCtrl() {
        return this.loginForm!.get('password');
    }
}