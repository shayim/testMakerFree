import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'app-auth',
    templateUrl: 'auth.component.html',
    styleUrls: ['auth.component.css']
})
export class AuthComponent implements OnInit {
    loginForm: FormGroup | undefined;
    constructor(private authService: AuthService, private fb: FormBuilder) { }

    ngOnInit() {
        this.loginForm = this.fb.group({
            username: ['', Validators.required],
            password: ['', Validators.required]
        });
    }

    login() {
        this.authService.login(this.loginForm!.value).subscribe(data => console.log(data, 'success'));
    }

    get usernameCtl() {
        return this.loginForm!.get('username');
    }

    get passwordCtrl() {
        return this.loginForm!.get('password');
    }
}