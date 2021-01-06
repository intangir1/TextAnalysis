package com.likhtman.TextAnalysis.Fragments;


import android.Manifest;
import android.app.Activity;
import android.content.ContentResolver;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.content.res.Resources;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Color;
import android.graphics.drawable.Drawable;
import android.net.Uri;
import android.os.Bundle;

import androidx.annotation.Nullable;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;
import androidx.fragment.app.Fragment;

import android.provider.MediaStore;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.Base64;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.google.android.material.textfield.TextInputEditText;
import com.google.android.material.textfield.TextInputLayout;
import com.likhtman.TextAnalysis.Logics.DrawableResize;
import com.likhtman.TextAnalysis.Logics.Validation;
import com.likhtman.TextAnalysis.Model.User;
import com.likhtman.TextAnalysis.R;
import com.likhtman.TextAnalysis.Services.UserService;
import com.squareup.picasso.Picasso;
import com.theartofdev.edmodo.cropper.CropImage;
import com.theartofdev.edmodo.cropper.CropImageView;
import com.wdullaer.materialdatetimepicker.date.DatePickerDialog;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.InputStream;
import java.net.URI;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.List;
import java.util.regex.Pattern;
import java.util.stream.IntStream;

import fr.ganfra.materialspinner.MaterialSpinner;
import jp.wasabeef.picasso.transformations.CropCircleTransformation;
import jp.wasabeef.picasso.transformations.CropSquareTransformation;

import static android.app.Activity.RESULT_OK;

public class SignUpFragment extends Fragment {
    final String TAG = SignUpFragment.this.getClass().getSimpleName();

    private Activity parentActivity;
    private View view;

    private TextInputLayout lay_name;
    private TextInputLayout lay_surname;
    private TextInputLayout lay_id;
    private TextInputLayout lay_birthday;
    private TextInputLayout lay_mail;
    private TextInputLayout lay_nick;
    private TextInputLayout lay_pass;
    private TextInputLayout lay_pass_again;

    private ImageView img_user_picture;

    private ImageButton btn_birthday;
    private Button btn_take_picture;
    private TextInputEditText etxt_birthday;

    private Button btn_enter;
    private Button btn_remove_picture;

    private MaterialSpinner spinner;
    private List<String> list_items = new ArrayList<String>();
    private ArrayAdapter<String> adapter;

    private int error_count;
    private boolean are_all_touched;
    private boolean is_gender_touched;

    private String userPicture;
    private String userImage;


    private final int MY_PERMISSIONS_REQUEST_USE_CROPPER = 0x00AF;


    private void Init_visuals(View view) {

        btn_birthday = (ImageButton) view.findViewById(R.id.btn_birthday);
        etxt_birthday = (TextInputEditText) view.findViewById((R.id.birthday_edit));

        lay_name = (TextInputLayout)view.findViewById(R.id.first_name_UP);
        lay_surname = (TextInputLayout)view.findViewById(R.id.last_name_UP);
        lay_id = (TextInputLayout)view.findViewById(R.id.teudat);
        lay_birthday = (TextInputLayout)view.findViewById(R.id.birthday);
        lay_mail = (TextInputLayout)view.findViewById(R.id.mail);
        lay_nick = (TextInputLayout)view.findViewById(R.id.user_name_UP);
        lay_pass = (TextInputLayout)view.findViewById(R.id.passUP);
        lay_pass_again = (TextInputLayout)view.findViewById(R.id.passAgain);

        spinner = (MaterialSpinner)view.findViewById(R.id.spnr_gender);

        btn_enter = (Button) view.findViewById(R.id.enterUP);

        btn_remove_picture = (Button) view.findViewById(R.id.btn_remove_picture);

        btn_take_picture = (Button) view.findViewById(R.id.btn_take_picture);

        img_user_picture = (ImageView) view.findViewById(R.id.img_user_picture);
    }


    private void Init_default(){
        btn_enter.setEnabled(false);
        error_count = 0;
        are_all_touched = false;
        is_gender_touched = false;
        btn_remove_picture.setVisibility(View.INVISIBLE);
        userPicture = "";
        userImage = "";
    }


    private void Init_listeners() {
        Basic_input_check(lay_name, 2);
        Basic_input_check(lay_surname, 2);
        Id_input_check(lay_id, 8);
        Birthday_input_check(lay_birthday);
        Gender_input_check(spinner);
        Mail_input_check(lay_mail, 2);
        Basic_input_check(lay_nick, 2);
        Pass_input_check(lay_pass, 2);
        Pass_again_input_check(lay_pass_again, 2);


        btn_take_picture.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Check_permissions_for_crop();
            }
        });


        Calendar calendar = Calendar.getInstance();
        final int day = calendar.get(Calendar.DAY_OF_MONTH);
        final int month = calendar.get(Calendar.MONTH);
        final int year = calendar.get(Calendar.YEAR);

        btn_birthday.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                DatePickerDialog dialog = DatePickerDialog.newInstance(new DatePickerDialog.OnDateSetListener() {
                    @Override
                    public void onDateSet(DatePickerDialog view, int year, int monthOfYear, int dayOfMonth) {
                        String str_date = dayOfMonth + "/" + (monthOfYear + 1) + "/" + year;
                        etxt_birthday.setText(str_date);
                    }
                }, year, month, day);
                dialog.show(parentActivity.getFragmentManager(), "");
            }
        });


        btn_remove_picture.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                btn_remove_picture.setVisibility(View.INVISIBLE);
                img_user_picture.setImageDrawable(null);
                userPicture = "";
                userImage = "";
            }
        });


        btn_enter.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                try {
                    User user = new User(
                            lay_id.getEditText().getText().toString(),
                            lay_name.getEditText().getText().toString(),
                            lay_surname.getEditText().getText().toString(),
                            lay_nick.getEditText().getText().toString(),
                            lay_pass.getEditText().getText().toString(),
                            lay_mail.getEditText().getText().toString(),
                            spinner.getItemAtPosition(spinner.getSelectedItemPosition()-1).toString(),
                            new SimpleDateFormat("dd/MM/yyyy").parse(etxt_birthday.getText().toString()),
                            userPicture,
                            (short)1,
                            "registrated",
                            userImage
                            );
                    UserService.Get_instance(parentActivity).SignUp(user);

                } catch (ParseException e) {
                    e.printStackTrace();
                }
            }
        });

    }


    private void Check_permissions_for_crop() {

        ArrayList<String> permissions_list = new ArrayList<String>();

        if(ContextCompat.checkSelfPermission(parentActivity, Manifest.permission.CAMERA ) != PackageManager.PERMISSION_GRANTED){
            permissions_list.add(Manifest.permission.CAMERA);
        }

        if(ContextCompat.checkSelfPermission(parentActivity, Manifest.permission.READ_EXTERNAL_STORAGE ) != PackageManager.PERMISSION_GRANTED){
            permissions_list.add(Manifest.permission.READ_EXTERNAL_STORAGE);
        }

        if(permissions_list.size() == 0){
            Activate_cropper();
        } else{
            String[] permissions = permissions_list.toArray(new String[0]);
            Log.d(TAG,"Not all permission available - requesting permissions");
            requestPermissions(permissions, MY_PERMISSIONS_REQUEST_USE_CROPPER);
        }

    }


    private void Activate_cropper(){
        Log.d(TAG,"Crop activity starting");
        CropImage.activity(null).setGuidelines(CropImageView.Guidelines.ON).setFixAspectRatio(true).start(getContext(), this);
    }


    private boolean Has_all_permissions(int[] grantResults){
        boolean contains_not_granted = IntStream.of(grantResults).anyMatch(x -> x != PackageManager.PERMISSION_GRANTED);
        return !contains_not_granted;
    }


    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        parentActivity = getActivity();

    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        return inflater.inflate(R.layout.sign_up_fragment, container, false);
    }


    @Override
    public void onViewCreated(View _view, Bundle savedInstanceState) {
        super.onViewCreated(_view, savedInstanceState);

        Init_visuals(_view);
        Init_default();
        Init_listeners();

        view = _view;
    }


    @Override
    public void onRequestPermissionsResult(int requestCode, String permissions[], int[] grantResults) {
        switch (requestCode) {
            case MY_PERMISSIONS_REQUEST_USE_CROPPER: {
                if(grantResults.length > 0 && Has_all_permissions(grantResults) ){
                    Log.d(TAG,"Cropper: All permissions received!");
                    Activate_cropper();
                } else{
                    Log.d(TAG,"Cropper: Permissions denied - not all permissions were given!");
                    Toast.makeText(parentActivity, "Not all permissions were given", Toast.LENGTH_SHORT).show();
                }
            }
        }
    }


    private void Result_crop(Intent data, int resultCode) {
        CropImage.ActivityResult result = CropImage.getActivityResult(data);
        if (resultCode == RESULT_OK) {
            Uri uri = result.getUri();

            if(CheckImageExtention(uri)){
                ConvertImageToByte(uri);
                btn_remove_picture.setVisibility(View.VISIBLE);
                int width = img_user_picture.getWidth();
                int height = img_user_picture.getHeight();

                Resources res = view.getContext().getResources();

                Drawable placeholder = DrawableResize.GetInstance(view.getContext()).Get_resize(res.getDrawable(R.drawable.placeholder), width, height);
                Drawable error = DrawableResize.GetInstance(view.getContext()).Get_resize(res.getDrawable(R.drawable.error_placeholder), width, height);
                Picasso.get()
                        .load(uri)
                        .placeholder(placeholder)
                        .error(error)
                        .resize(width, height)
                        .transform(new CropSquareTransformation())
                        .into(img_user_picture);

            } else{
                Log.e(TAG,"Wrong image extension");
                Toast.makeText(parentActivity, "Image extension not acceptable", Toast.LENGTH_SHORT).show();
            }

        } else if (resultCode == CropImage.CROP_IMAGE_ACTIVITY_RESULT_ERROR_CODE) {
            Log.e(TAG,"Cropping error");
            Toast.makeText(parentActivity, "Cropping failed: " + result.getError(), Toast.LENGTH_LONG).show();
        }
    }


    @Override
    public void onActivityResult(int requestCode, int resultCode, @Nullable Intent data) {
        super.onActivityResult(requestCode, resultCode, data);

        switch (requestCode){
            case CropImage.CROP_IMAGE_ACTIVITY_REQUEST_CODE: {
                Result_crop(data, resultCode);
                return;
            }
        }
    }


    private void ConvertImageToByte(Uri uri){
        try {
            ContentResolver cr = parentActivity.getContentResolver();
            InputStream inputStream = cr.openInputStream(uri);
            Bitmap bitmap = BitmapFactory.decodeStream(inputStream);
            ByteArrayOutputStream baos = new ByteArrayOutputStream();
            bitmap.compress(Bitmap.CompressFormat.JPEG, 100, baos);
            byte[] data = baos.toByteArray();
            this.userImage = Base64.encodeToString(data, Base64.DEFAULT);
            Log.d(TAG,"Convert to byte successful");
        } catch (FileNotFoundException e) {
            e.printStackTrace();
            Log.e(TAG,"Convert to byte error");
        }
    }

    private boolean CheckImageExtention(Uri uri){
        File file= new File(uri.getPath());
        String tempPictureName = file.getName();
        String rgx_acceptable_extention = "(jpg|png|jpeg)$";
        if(Pattern.compile(rgx_acceptable_extention).matcher(tempPictureName).find())
        {
            userPicture = tempPictureName;
            return true;
        }
        return false;
    }


    private boolean Errors_update_basic(TextInputLayout lay, int minimumChars) {

        if(lay.getEditText().getText().toString().length() == 0){
            lay.setError("Input required");
            return true;
        } else if(lay.getEditText().getText().toString().length() < minimumChars) {
            lay.setError("Minimum " + minimumChars + " characters required");
            return true;
        }

        return false;
    }


    private void Basic_input_check(TextInputLayout lay, int min_chars) {
        lay.getEditText().addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

                boolean is_set_error = lay.isErrorEnabled();
                boolean has_basic_error = Errors_update_basic(lay, min_chars);

                if(is_set_error && !has_basic_error){
                    lay.setErrorEnabled(false);
                    error_count--;
                } else if(!is_set_error && has_basic_error){
                    lay.setErrorEnabled(true);
                    error_count++;
                }

                Button_enter_check();

            }

            @Override
            public void afterTextChanged(Editable s) {

            }
        });
    }


    private boolean Errors_update_id(TextInputLayout lay) {

        if(!Validation.Get_instance().Id_is_valid(lay.getEditText().getText().toString())) {
            lay.setError("Bad id!");
            return true;
        }

        return false;
    }


    private void Id_input_check(TextInputLayout lay, int min_chars) {
        lay.getEditText().addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {
                boolean is_set_error = lay.isErrorEnabled();
                boolean has_basic_error = Errors_update_basic(lay, min_chars);
                boolean has_id_error = false;
                if(!has_basic_error){
                    has_id_error = Errors_update_id(lay);
                }

                if(is_set_error && !has_basic_error && !has_id_error){
                    lay.setErrorEnabled(false);
                    error_count--;
                } else if (!is_set_error && (has_basic_error || has_id_error) ){
                    lay.setErrorEnabled(true);
                    error_count++;
                }

                Button_enter_check();
            }

            @Override
            public void afterTextChanged(Editable s) {

            }
        });
    }


    private boolean Errors_update_date(TextInputLayout lay) {
        if(lay.getEditText().getText().toString().length() == 0){
            lay.setError("Input required");
            return true;
        }

        SimpleDateFormat dateFormat = new SimpleDateFormat("dd/MM/yyyy");
        dateFormat.setLenient(false);
        Date date = null;
        try {
            date = dateFormat.parse(lay.getEditText().getText().toString().trim());
            Log.d(TAG,"Date set successfully");
        } catch (ParseException e) {
            lay.setError("Wrong date format");
            Log.e(TAG,"Wrong date format");
            return true;
        }

        if(date.after(new Date())){
            lay.setError("Wrong time input");
            return true;
        }

        return false;
    }


    private void Birthday_input_check(TextInputLayout lay) {
        lay.getEditText().addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {
                boolean is_set_error = lay.isErrorEnabled();
                boolean has_date_error = Errors_update_date(lay);

                if(is_set_error && !has_date_error){
                    lay.setErrorEnabled(false);
                    error_count--;
                } else if (!is_set_error && has_date_error){
                    lay.setErrorEnabled(true);
                    error_count++;
                }

                Button_enter_check();
            }

            @Override
            public void afterTextChanged(Editable s) {

            }
        });
    }


    private void Color_gender_error(){
        spinner.setError("Gender not chosen");
        spinner.setErrorColor(Color.RED);
        ((TextView) spinner.getChildAt(0)).setTextColor(Color.RED);

    }


    private void Gender_input_check(MaterialSpinner spinner) {
        list_items.add("Male");
        list_items.add("Female");
        list_items.add("Other");


        adapter = new ArrayAdapter<String>(parentActivity, android.R.layout.simple_spinner_dropdown_item, list_items);
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        spinner.setAdapter(adapter);

        spinner.setOnTouchListener(new View.OnTouchListener() {
            @Override
            public boolean onTouch(View v, MotionEvent event) {
                if(!is_gender_touched && event.getAction() == MotionEvent.ACTION_UP){
                    is_gender_touched = true;
                    Color_gender_error();
                }
                return false;
            }
        });


        spinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {



                if(position < 0 && is_gender_touched){
                    Color_gender_error();
                } else {
                    spinner.setError(null);
                }

                Button_enter_check();

            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });
    }


    private Boolean Errors_update_mail(TextInputLayout lay){
        if(Validation.Get_instance().Email_is_valid(lay.getEditText().getText().toString())){
            return false;
        }
        lay.setError("Wrong mail format");
        return true;
    }


    private void Mail_input_check(TextInputLayout lay, int min_chars) {
        lay.getEditText().addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {
                boolean is_set_error = lay.isErrorEnabled();
                boolean has_basic_error = Errors_update_basic(lay, min_chars);
                boolean has_mail_error = false;
                if(!has_basic_error){
                    has_mail_error = Errors_update_mail(lay);
                }

                if(is_set_error && !has_basic_error && !has_mail_error){
                    lay.setErrorEnabled(false);
                    error_count--;
                } else if (!is_set_error && (has_basic_error || has_mail_error)){
                    lay.setErrorEnabled(true);
                    error_count++;
                }

                Button_enter_check();
            }

            @Override
            public void afterTextChanged(Editable s) {

            }
        });
    }


    private void Pass_input_check(TextInputLayout lay, int min_chars) {
        lay.getEditText().addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

                boolean is_set_error = lay.isErrorEnabled();
                boolean has_basic_error = Errors_update_basic(lay, min_chars);


                if(is_set_error && !has_basic_error){
                    lay.setErrorEnabled(false);
                    error_count--;
                } else if(!is_set_error && has_basic_error){
                    lay.setErrorEnabled(true);
                    error_count++;
                }

                String again_pass = lay_pass_again.getEditText().getText().toString();
                if(again_pass.length() > 0){
                    lay_pass_again.getEditText().setText(again_pass);
                }

                Button_enter_check();

            }

            @Override
            public void afterTextChanged(Editable s) {

            }
        });
    }


    private boolean Errors_update_pass(TextInputLayout lay) {
        String origin_pass = lay_pass.getEditText().getText().toString();
        if(origin_pass.length() == 0){
            lay.setError("Input pass first");
            return true;
        }

        if(!origin_pass.equals(lay.getEditText().getText().toString())){
            lay.setError("Passes are different");
            return true;
        }

        return false;
    }


    private void Pass_again_input_check(TextInputLayout lay, int min_chars) {
        lay.getEditText().addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

                boolean is_set_error = lay.isErrorEnabled();
                boolean has_basic_error = Errors_update_basic(lay, min_chars);
                boolean has_pass_error = false;
                if(!has_basic_error){
                    has_pass_error = Errors_update_pass(lay);
                }

                if(is_set_error && !has_basic_error && !has_pass_error){
                    lay.setErrorEnabled(false);
                    error_count--;
                } else if(!is_set_error && (has_basic_error || has_pass_error)){
                    lay.setErrorEnabled(true);
                    error_count++;
                }

                Button_enter_check();

            }

            @Override
            public void afterTextChanged(Editable s) {

            }
        });
    }


    private boolean Is_touched(TextInputLayout lay){
        if(lay.getEditText().getText().toString().length() == 0 && !lay.isErrorEnabled()){
            return false;
        }
        return true;
    }


    private boolean Check_all_touched() {

        if (
                Is_touched(lay_name) && Is_touched(lay_surname) && Is_touched(lay_id) &&
                        Is_touched(lay_birthday) && Is_touched(lay_mail) && Is_touched(lay_nick) &&
                        Is_touched(lay_pass) && Is_touched(lay_pass_again)
        ) {
            return true;
        }
        return false;

    }


    private void Button_enter_update(){

        if(spinner.getSelectedItemPosition() > 0 && error_count == 0){
            btn_enter.setEnabled(true);
        } else{
            btn_enter.setEnabled(false);
        }

    }


    private void Button_enter_check(){
        if(is_gender_touched){
            if(!are_all_touched){
                are_all_touched = Check_all_touched();
                if(are_all_touched){
                    Button_enter_update();
                }
            } else{
                Button_enter_update();
            }
        }
    }
}