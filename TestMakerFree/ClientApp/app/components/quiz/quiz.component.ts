import { Component, Input } from '@angular/core';
import { IQuiz } from "../../Interfaces/quiz";

@Component({
    selector: 'app-quiz',
    template: `
        <div>
            <h2>
               <a [routerLink]="['/quiz', quiz.id ]"> {{quiz.title}} </a>
            </h2>
            <p>
                {{quiz.description}}
            </p>
        </div>
`,
    styles: [``]
})
export class QuizComponent {
    @Input() quiz: IQuiz | undefined;
}