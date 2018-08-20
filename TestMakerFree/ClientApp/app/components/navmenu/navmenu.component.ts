import { Component } from '@angular/core';
import { AuthService } from '../../modules/auth/services/auth.service';

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent {
    constructor(public auth: AuthService) { }
}