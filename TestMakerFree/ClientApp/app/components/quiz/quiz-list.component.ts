import { Component, OnInit, Input } from '@angular/core'
import { QuizService } from '../../services/quiz.service';
import { IQuiz } from '../../interfaces/quiz'

@Component({
    selector: 'app-quiz-list',
    template: `
<h1> {{title}}</h1>
<ul>
    <li *ngFor="let quiz of quizzes" [class.selected]="quiz === selected" (click)="select(quiz)">
        <app-quiz [quiz]="quiz"></app-quiz>
    </li>
</ul>
`,
    styleUrls: ['quiz-list.component.css']
})
export class QuizListComponent implements OnInit {
    @Input() class: string | undefined;
    @Input() itemNum: number = 10;
    selected: IQuiz | undefined;
    quizzes: Array<IQuiz> = [];
    title = 'Quizzes';
    constructor(private quizService: QuizService) { }

    ngOnInit() {
        this.getQizzes();
    }

    select(quiz: IQuiz) {
        this.selected = quiz;
    }

    private getQizzes() {
        switch (this.class) {
            case 'latest':
                this.title = `${this.title} Latest`;
                break;
            case 'byTitle':
                this.title = `${this.title} by Title`;
                break;
            case 'byRandom':
                this.title = `${this.title} by Random`;
                break;
        }
        this.quizService.getQuizzes(this.class!, this.itemNum).subscribe(data => {
            this.quizzes = data;
        }, error => console.log(error));
    }
}