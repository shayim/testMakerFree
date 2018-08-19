import { Component } from '@angular/core';
import { IQuestion } from '../../Interfaces/question'
import { QuizService } from '../../services/quiz.service'
import { ActivatedRoute, Router } from '@angular/router';
import { map, switchMap } from 'rxjs/operators';
import { of } from 'rxjs';

@Component({
    selector: 'app-question-editor',
    templateUrl: `question-editor.component.html`,
    styles: [``]
})
export class QuestionEditorComponent {
    title = 'Edit';
    question: IQuestion | undefined;

    constructor(private quizService: QuizService, private route: ActivatedRoute, private router: Router) { }

    ngOnInit() {
        this.route.paramMap.pipe(map(params => {
            return { quizId: +params.get('id')!, id: +params.get('qid')! }
        }),
            switchMap(ids => {
                if (ids.id) {
                    return this.quizService.getQuestion(ids.quizId, ids.id);
                } else {
                    this.title = "Create";
                    const q = { quizId: ids.quizId, id: ids.id } as IQuestion;
                    return of(q);
                }
            }))
            .subscribe(data => this.question = data);
    }

    submit(question: IQuestion) {
        this.quizService.postOrUpdateQuestion(question)
            .subscribe(_ => {
                this.router.navigate(['/quiz', this.question!.quizId!]);
            }, error => console.log(error));
    }

    remove() {
        if (confirm('are you sure to delete?')) {
            this.quizService.removeQuestion(this.question!.quizId, this.question!.id)
                .subscribe(_ => this.router.navigate(['/quiz', this.question!.quizId]),
                    error => console.log(error));
        }
    }
}