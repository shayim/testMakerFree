import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { QuizListComponent } from './components/quiz/quiz-list.component';
import { QuizComponent } from './components/quiz/quiz.component';
import { QuizEditorComponent } from './components/quiz/quiz-editor.component';
import { AboutComponent } from './components/about/about.component';
import { LoginComponent } from './components/login/login.component';
import { NotFoundComponent } from './components/not-found/not-found.component'
import { QuestionListComponent } from './components/question/question-list.component';
import { QuestionEditorComponent } from './components/question/question-editor.component';
import { ResultListComponent } from './components/result/result-list.component';
import { ResultEditorComponent } from './components/result/result-editor.component';
import { AnswerListComponent } from './components/answer/answer-list.component';
import { AnswerEditorComponent } from './components/answer/answer-editor.component';
import { AuthModule } from './modules/auth/auth.module';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        AboutComponent,
        LoginComponent,
        QuizListComponent,
        QuizComponent,
        QuizEditorComponent,
        QuestionListComponent,
        QuestionEditorComponent,
        ResultListComponent,
        ResultEditorComponent,
        AnswerListComponent,
        AnswerEditorComponent,
        NotFoundComponent
    ],
    imports: [
        AuthModule,
        CommonModule,
        HttpClientModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'login', component: LoginComponent },
            { path: 'about', component: AboutComponent },
            { path: 'quiz/:id', component: QuizEditorComponent },
            { path: 'quiz/:id/questions/:qid', component: QuestionEditorComponent },
            { path: 'quiz/:id/questions/:qid/answers/:aid', component: AnswerEditorComponent },
            { path: 'quiz/:id/results/:rid', component: ResultEditorComponent },
            { path: '**', component: NotFoundComponent }
        ])
    ]
})
export class AppModuleShared {
}